using Aspirant.Hosting;
using Aspire.Hosting.ApplicationModel;

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
        var resource = new YarpResource(name);

        return builder.AddWebApplication<YarpResource, YarpResourceLifecycleHook>(resource, excludeFromManifest: true);
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
