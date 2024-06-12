using Aspire.Hosting;
using Aspire.Hosting.ApplicationModel;
using Aspire.Hosting.Lifecycle;

namespace Aspirant.Hosting;


/// <summary>
/// Extensions method to add WebApplication resources.
/// </summary>
public static class WebApplicationResourceExtensions
{
    public static IResourceBuilder<TResource> AddWebApplication<TResource, TLifecycleHook>(this IDistributedApplicationBuilder builder, TResource resource, bool excludeFromManifest = false)
        where TResource : WebApplicationResource
        where TLifecycleHook : WebApplicationResourceLifecycleHook<TResource>
    {
        var webApplicationResource = builder.Resources.OfType<TResource>().SingleOrDefault();

        if (webApplicationResource is not null)
        {
            throw new InvalidOperationException($"A resource of type {nameof(TResource)} has already been added to this application");
        }

        builder.Services.TryAddLifecycleHook<TLifecycleHook>();

        var resourceBuilder = builder.AddResource(resource);

        if (excludeFromManifest)
        {
            resourceBuilder = resourceBuilder.ExcludeFromManifest();
        }

        return resourceBuilder;
    }
}