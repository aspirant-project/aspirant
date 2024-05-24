using Aspire.Hosting.ApplicationModel;
using Aspire.Hosting.Lifecycle;

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
