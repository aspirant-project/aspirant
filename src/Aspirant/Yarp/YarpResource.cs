using System.Linq;
using Aspire.Hosting.ApplicationModel;
using Aspire.Hosting.Lifecycle;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.Hosting.Server.Features;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Yarp.ReverseProxy.Configuration;

namespace Aspire.Hosting;

/// <summary>
/// Extensions for the <see cref="YarpResource"/>.
/// </summary>
public static class YarpResourceExtensions
{
    /// <summary>
    /// Adds a YARP resource to the application.
    /// </summary>
    /// <param name="builder">The builder.</param>
    /// <param name="name">The name of the resource.</param>
    /// <returns>The builder.</returns>
    /// <exception cref="InvalidOperationException"></exception>
    public static IResourceBuilder<YarpResource> AddYarp(this IDistributedApplicationBuilder builder, string name)
    {
        var yarp = builder.Resources.OfType<YarpResource>().SingleOrDefault();

        if (yarp is not null)
        {
            // You only need one yarp resource per application
            throw new InvalidOperationException("A yarp resource has already been added to this application");
        }

        builder.Services.TryAddLifecycleHook<YarpResourceLifecyclehook>();

        var resource = new YarpResource(name);
        return builder.AddResource(resource).ExcludeFromManifest();

        // REVIEW: YARP resource type?
        //.WithManifestPublishingCallback(context =>
        // {
        //     context.Writer.WriteString("type", "yarp.v0");

        //     context.Writer.WriteStartObject("routes");
        //     // REVIEW: Make this less YARP specific
        //     foreach (var r in resource.RouteConfigs.Values)
        //     {
        //         context.Writer.WriteStartObject(r.RouteId);

        //         context.Writer.WriteStartObject("match");
        //         context.Writer.WriteString("path", r.Match.Path);

        //         if (r.Match.Hosts is not null)
        //         {
        //             context.Writer.WriteStartArray("hosts");
        //             foreach (var h in r.Match.Hosts)
        //             {
        //                 context.Writer.WriteStringValue(h);
        //             }
        //             context.Writer.WriteEndArray();
        //         }
        //         context.Writer.WriteEndObject();
        //         context.Writer.WriteString("destination", r.ClusterId);
        //         context.Writer.WriteEndObject();
        //     }
        //     context.Writer.WriteEndObject();
        // });
    }

    /// <summary>
    /// Loads the YARP configuration from the specified configuration section.
    /// </summary>
    /// <param name="builder">The builder.</param>
    /// <param name="sectionName">The configuration section name to load from.</param>
    /// <returns>The builder.</returns>
    public static IResourceBuilder<YarpResource> LoadFromConfiguration(this IResourceBuilder<YarpResource> builder, string sectionName)
    {
        builder.Resource.ConfigurationSectionName = sectionName;
        return builder;
    }
}

/// <summary>
/// Represents a YARP resource.
/// </summary>
/// <param name="name">The name of the resource in the application model.</param>
public class YarpResource(string name) : Resource(name), IResourceWithServiceDiscovery, IResourceWithEnvironment
{
    // YARP configuration
    internal Dictionary<string, RouteConfig> RouteConfigs { get; } = [];
    internal Dictionary<string, ClusterConfig> ClusterConfigs { get; } = [];
    //internal List<EndpointAnnotation> Endpoints { get; } = [];
    internal string? ConfigurationSectionName { get; set; }
}

// This starts up the YARP reverse proxy with the configuration from the resource
internal class YarpResourceLifecyclehook(
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

        // We don't want to create proxies for yarp resources so remove them
        var bindings = yarpResource.Annotations.OfType<EndpointAnnotation>().ToList();

        foreach (var b in bindings)
        {
            //yarpResource.Annotations.Remove(b);
            b.IsProxied = false;
            //yarpResource.Endpoints.Add(b);
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

        var builder = WebApplication.CreateSlimBuilder();

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

        _app = builder.Build();

        var urlToEndpointNameMap = new Dictionary<string, string>();

        var defaultScheme = Environment.GetEnvironmentVariable("ASPNETCORE_URLS")?.Contains("https://") == true ? "https" : "http";

        if (!yarpResource.TryGetEndpoints(out var endpoints))
        {
            var url = "http://127.0.0.1:0";
            _app.Urls.Add(url);
            urlToEndpointNameMap[url] = "default";
        }
        else
        {
            foreach (var ep in endpoints)
            {
                var scheme = ep.UriScheme ?? defaultScheme;
                var url = ep.Port switch
                {
                    null => $"{scheme}://127.0.0.1:0",
                    _ => $"{scheme}://localhost:{ep.Port}"
                };

                _app.Urls.Add(url);
                urlToEndpointNameMap[new Uri(url).ToString()] = ep.Name;
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
                    ep.AllocatedEndpoint = new(ep, uri.Host, uri.Port);
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