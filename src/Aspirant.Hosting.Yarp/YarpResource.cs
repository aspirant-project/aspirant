using System.Linq;
using Aspire.Hosting.ApplicationModel;
using Aspire.Hosting.Lifecycle;
using k8s.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.Hosting.Server.Features;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Yarp.ReverseProxy.Configuration;

namespace Aspire.Hosting;

/// <summary>
/// Represents a YARP resource.
/// </summary>
/// <param name="name">The name of the resource in the application model.</param>
public class YarpResource(string name) : Resource(name), IResourceWithServiceDiscovery, IResourceWithEnvironment
{
    // YARP configuration
    internal Dictionary<string, RouteConfig> RouteConfigs { get; } = [];
    internal Dictionary<string, ClusterConfig> ClusterConfigs { get; } = [];
    internal string? ConfigurationSectionName { get; set; }
}

// This starts up the YARP reverse proxy with the configuration from the resource
internal class YarpResourceLifecyclehook(
    IHostEnvironment hostEnvironment,
    DistributedApplicationExecutionContext executionContext,
    ResourceNotificationService resourceNotificationService,
    ResourceLoggerService resourceLoggerService) : IDistributedApplicationLifecycleHook, IAsyncDisposable
{
    private WebApplication? _app;

    public async Task BeforeStartAsync(DistributedApplicationModel appModel, CancellationToken cancellationToken = default)
    {
        if (executionContext.IsPublishMode)
        {
            return;
        }

        var yarpResource = appModel.Resources.OfType<YarpResource>().SingleOrDefault();

        if (yarpResource is null)
        {
            return;
        }

        await resourceNotificationService.PublishUpdateAsync(yarpResource, s => s with
        {
            ResourceType = "Yarp",
            State = "Starting"
        });

        // We don't want to proxy for yarp resources so force endpoints to not proxy
        var bindings = yarpResource.Annotations.OfType<EndpointAnnotation>().ToList();

        foreach (var b in bindings)
        {
            b.IsProxied = false;
        }
    }

    public async Task AfterEndpointsAllocatedAsync(DistributedApplicationModel appModel, CancellationToken cancellationToken = default)
    {
        if (executionContext.IsPublishMode)
        {
            return;
        }

        var yarpResource = appModel.Resources.OfType<YarpResource>().SingleOrDefault();

        if (yarpResource is null)
        {
            return;
        }
        var builder = WebApplication.CreateSlimBuilder(new WebApplicationOptions
        {
            ContentRootPath = hostEnvironment.ContentRootPath,
            EnvironmentName = hostEnvironment.EnvironmentName, 
            WebRootPath = Path.Combine(hostEnvironment.ContentRootPath, "wwwroot")
        });

        builder.Logging.ClearProviders();

        builder.Logging.AddProvider(new ResourceLoggerProvider(resourceLoggerService.GetLogger(yarpResource.Name)));

        // Convert environment variables into configuration
        if (yarpResource.TryGetEnvironmentVariables(out var envAnnotations))
        {
            var context = new EnvironmentCallbackContext(executionContext, cancellationToken: cancellationToken);

            foreach (var cb in envAnnotations)
            {
                await cb.Callback(context);
            }

            var dict = new Dictionary<string, string?>();
            foreach (var (k, v) in context.EnvironmentVariables)
            {
                var val = v switch
                {
                    string s => s,
                    IValueProvider vp => await vp.GetValueAsync(context.CancellationToken),
                    _ => throw new NotSupportedException()
                };

                if (val is not null)
                {
                    dict[k.Replace("__", ":")] = val;
                }
            }

            builder.Configuration.AddInMemoryCollection(dict);
        }

        builder.Services.AddServiceDiscovery();

        var proxyBuilder = builder.Services.AddReverseProxy();

        if (yarpResource.RouteConfigs.Count > 0)
        {
            proxyBuilder.LoadFromMemory([.. yarpResource.RouteConfigs.Values], [.. yarpResource.ClusterConfigs.Values]);
        }

        if (yarpResource.ConfigurationSectionName is not null)
        {
            proxyBuilder.LoadFromConfig(builder.Configuration.GetSection(yarpResource.ConfigurationSectionName));
        }

        proxyBuilder.AddServiceDiscoveryDestinationResolver();

        yarpResource.TryGetEndpoints(out var endpoints);
        var defaultScheme = Environment.GetEnvironmentVariable("ASPNETCORE_URLS")?.Contains("https://") == true ? "https" : "http";
        var needHttps = defaultScheme == "https" || endpoints?.Any(ep => ep.UriScheme == "https") == true;

        if (needHttps)
        {
            builder.WebHost.UseKestrelHttpsConfiguration();
        }

        _app = builder.Build();

        var urlToEndpointNameMap = new Dictionary<string, string>();

        if (endpoints is null)
        {
            var url = $"{defaultScheme}://127.0.0.1:0/";
            _app.Urls.Add(url);
            urlToEndpointNameMap[url] = "default";
        }
        else
        {
            foreach (var ep in endpoints)
            {
                var scheme = ep.UriScheme ?? defaultScheme;
                needHttps = needHttps || scheme == "https";

                var url = ep.Port switch
                {
                    null => $"{scheme}://127.0.0.1:0/",
                    _ => $"{scheme}://localhost:{ep.Port}"
                };

                var uri = new Uri(url);
                _app.Urls.Add(url);
                urlToEndpointNameMap[uri.ToString()] = ep.Name;
            }
        }

        _app.MapReverseProxy();

        await _app.StartAsync(cancellationToken);

        var addresses = _app.Services.GetRequiredService<IServer>().Features.GetRequiredFeature<IServerAddressesFeature>().Addresses;

        // Update the EndpointAnnotations with the allocated URLs from ASP.NET Core
        foreach (var url in addresses)
        {
            if (urlToEndpointNameMap.TryGetValue(new Uri(url).ToString(), out var name)
                || urlToEndpointNameMap.TryGetValue((new UriBuilder(url) { Port = 0 }).Uri.ToString(), out name))
            {
                var ep = endpoints?.FirstOrDefault(ep => ep.Name == name);
                if (ep is not null)
                {
                    var uri = new Uri(url);
                    var host = uri.Host is "127.0.0.1" or "[::1]" ? "localhost" : uri.Host;
                    ep.AllocatedEndpoint = new(ep, host, uri.Port);
                }
            }
        }

        await resourceNotificationService.PublishUpdateAsync(yarpResource, s => s with
        {
            State = "Running",
            Urls = [.. endpoints?.Select(ep => new UrlSnapshot(ep.Name, ep.AllocatedEndpoint?.UriString ?? "", IsInternal: false))],
        });
    }

    public ValueTask DisposeAsync()
    {
        return _app?.DisposeAsync() ?? default;
    }

    private class ResourceLoggerProvider(ILogger logger) : ILoggerProvider
    {
        public ILogger CreateLogger(string categoryName)
        {
            return new ResourceLogger(logger);
        }

        public void Dispose()
        {
        }

        private class ResourceLogger(ILogger logger) : ILogger
        {
            public IDisposable? BeginScope<TState>(TState state) where TState : notnull
            {
                return logger.BeginScope(state);
            }

            public bool IsEnabled(LogLevel logLevel)
            {
                return logger.IsEnabled(logLevel);
            }

            public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception? exception, Func<TState, Exception?, string> formatter)
            {
                logger.Log(logLevel, eventId, state, exception, formatter);
            }
        }
    }
}