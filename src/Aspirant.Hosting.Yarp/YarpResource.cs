using Aspire.Hosting.ApplicationModel;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Yarp.ReverseProxy.Configuration;

using Aspirant.Hosting;

namespace Aspire.Hosting;

/// <summary>
/// Represents a YARP resource.
/// </summary>
/// <param name="name">The name of the resource in the application model.</param>
public class YarpResource(string name) : WebApplicationResource(name)
{
    internal Dictionary<string, RouteConfig> RouteConfigs { get; } = [];
    internal Dictionary<string, ClusterConfig> ClusterConfigs { get; } = [];
    internal string? ConfigurationSectionName { get; set; }
}

internal class YarpResourceLifecycleHook(IHostEnvironment hostEnvironment, DistributedApplicationExecutionContext executionContext, ResourceNotificationService resourceNotificationService, ResourceLoggerService resourceLoggerService) : WebApplicationResourceLifecycleHook<YarpResource>(hostEnvironment, executionContext, resourceNotificationService, resourceLoggerService)
{
    protected override string ResourceTypeName { get; } = "Yarp";

    protected override ValueTask ConfigureApplicationAsync(WebApplication app, YarpResource resource, CancellationToken cancellationToken)
    {
        app.MapReverseProxy();

        return ValueTask.CompletedTask;
    }

    protected override ValueTask ConfigureBuilderAsync(WebApplicationBuilder builder, YarpResource resource, CancellationToken cancellationToken)
    {
        builder.Services.AddServiceDiscovery();

        var proxyBuilder = builder.Services.AddReverseProxy();

        if (resource.RouteConfigs.Count > 0)
        {
            proxyBuilder.LoadFromMemory([.. resource.RouteConfigs.Values], [.. resource.ClusterConfigs.Values]);
        }

        if (resource.ConfigurationSectionName is not null)
        {
            proxyBuilder.LoadFromConfig(builder.Configuration.GetSection(resource.ConfigurationSectionName));
        }

        proxyBuilder.AddServiceDiscoveryDestinationResolver();

        return ValueTask.CompletedTask;
    }
}
