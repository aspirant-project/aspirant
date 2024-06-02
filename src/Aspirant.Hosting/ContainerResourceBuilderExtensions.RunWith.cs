using Aspire.Hosting.ApplicationModel;
using Aspire.Hosting;

namespace Aspirant.Hosting;

/// <summary>
/// Extension methods for <see cref="IResourceBuilder{ContainerResource}"/>.
/// </summary>
public static partial class ContainerResourceBuilderExtensions
{
    /// <summary>
    /// Adds a bind mount to a container resource when <see cref="DistributedApplicationExecutionContext.IsRunMode"/> is <c>true</c>.
    /// </summary>
    /// <typeparam name="TResource"></typeparam>
    /// <param name="builder">The resource builder.</param>
    /// <param name="source">The source path of the mount. This is the path to the file or directory on the host.</param>
    /// <param name="target">The target path where the file or directory is mounted in the container.</param>
    /// <param name="isReadOnly">A flag that indicates if this is a read-only mount.</param>
    /// <returns>The <see cref="IResourceBuilder{T}"/>.</returns>
    public static IResourceBuilder<TResource> RunWithBindMount<TResource>(this IResourceBuilder<TResource> builder, string source, string target, bool isReadOnly = false)
        where TResource : ContainerResource
    {
        if (builder.ApplicationBuilder.ExecutionContext.IsRunMode)
        {
            builder.WithBindMount(source, target, isReadOnly);
        }

        return builder;
    }

    /// <summary>
    /// Adds a volume to a container resource when <see cref="DistributedApplicationExecutionContext.IsRunMode"/> is <c>true</c>.
    /// </summary>
    /// <typeparam name="T">The resource type.</typeparam>
    /// <param name="builder">The resource builder.</param>
    /// <param name="name">The name of the volume.</param>
    /// <param name="target">The target path where the volume is mounted in the container.</param>
    /// <param name="isReadOnly">A flag that indicates if the volume should be mounted as read-only.</param>
    /// <returns>The <see cref="IResourceBuilder{T}"/>.</returns>
    public static IResourceBuilder<T> RunWithVolume<T>(this IResourceBuilder<T> builder, string name, string target, bool isReadOnly = false) where T : ContainerResource
    {
        if (builder.ApplicationBuilder.ExecutionContext.IsRunMode)
        {
            builder.WithVolume(name, target, isReadOnly);
        }

        return builder;
    }

    /// <summary>
    /// Adds an anonymous volume to a container resource when <see cref="DistributedApplicationExecutionContext.IsRunMode"/> is <c>true</c>.
    /// </summary>
    /// <typeparam name="T">The resource type.</typeparam>
    /// <param name="builder">The resource builder.</param>
    /// <param name="target">The target path where the volume is mounted in the container.</param>
    /// <returns>The <see cref="IResourceBuilder{T}"/>.</returns>
    public static IResourceBuilder<T> RunWithVolume<T>(this IResourceBuilder<T> builder, string target) where T : ContainerResource
    {
        if (builder.ApplicationBuilder.ExecutionContext.IsRunMode)
        {
            builder.WithVolume(target);
        }

        return builder;
    }

    /// <summary>
    /// Sets the Entrypoint for the container when <see cref="DistributedApplicationExecutionContext.IsRunMode"/> is <c>true</c>.
    /// </summary>
    /// <typeparam name="T">The resource type.</typeparam>
    /// <param name="builder">The resource builder.</param>
    /// <param name="entrypoint">The new entrypoint for the container.</param>
    /// <returns>The <see cref="IResourceBuilder{T}"/>.</returns>
    public static IResourceBuilder<T> RunWithEntrypoint<T>(this IResourceBuilder<T> builder, string entrypoint) where T : ContainerResource
    {
        if (builder.ApplicationBuilder.ExecutionContext.IsRunMode)
        {
            builder.WithEntrypoint(entrypoint);
        }

        return builder;
    }

    /// <summary>
    /// Allows overriding the image tag on a container when <see cref="DistributedApplicationExecutionContext.IsRunMode"/> is <c>true</c>.
    /// </summary>
    /// <typeparam name="T">Type of container resource.</typeparam>
    /// <param name="builder">Builder for the container resource.</param>
    /// <param name="tag">Tag value.</param>
    /// <returns>The <see cref="IResourceBuilder{T}"/>.</returns>
    public static IResourceBuilder<T> RunWithImageTag<T>(this IResourceBuilder<T> builder, string tag) where T : ContainerResource
    {
        if (builder.ApplicationBuilder.ExecutionContext.IsRunMode)
        {
            builder.WithImageTag(tag);
        }

        return builder;
    }

    /// <summary>
    /// Allows overriding the image registry on a container when <see cref="DistributedApplicationExecutionContext.IsRunMode"/> is <c>true</c>.
    /// </summary>
    /// <typeparam name="T">Type of container resource.</typeparam>
    /// <param name="builder">Builder for the container resource.</param>
    /// <param name="registry">Registry value.</param>
    /// <returns>The <see cref="IResourceBuilder{T}"/>.</returns>
    public static IResourceBuilder<T> RunWithImageRegistry<T>(this IResourceBuilder<T> builder, string registry) where T : ContainerResource
    {
        if (builder.ApplicationBuilder.ExecutionContext.IsRunMode)
        {
            builder.WithImageRegistry(registry);
        }

        return builder;
    }

    /// <summary>
    /// Allows overriding the image on a container when <see cref="DistributedApplicationExecutionContext.IsRunMode"/> is <c>true</c>.
    /// </summary>
    /// <typeparam name="T">Type of container resource.</typeparam>
    /// <param name="builder">Builder for the container resource.</param>
    /// <param name="image">Image value.</param>
    /// <param name="tag">Tag value.</param>
    /// <returns></returns>
    public static IResourceBuilder<T> RunWithImage<T>(this IResourceBuilder<T> builder, string image, string tag = "latest") where T : ContainerResource
    {
        if (builder.ApplicationBuilder.ExecutionContext.IsRunMode)
        {
            builder.WithImage(image, tag);
        }

        return builder;
    }

    /// <summary>
    /// Allows setting the image to a specific sha256 on a container when <see cref="DistributedApplicationExecutionContext.IsRunMode"/> is <c>true</c>.
    /// </summary>
    /// <typeparam name="T">Type of container resource.</typeparam>
    /// <param name="builder">Builder for the container resource.</param>
    /// <param name="sha256">Registry value.</param>
    /// <returns></returns>
    public static IResourceBuilder<T>RunWithImageSHA256<T>(this IResourceBuilder<T> builder, string sha256) where T : ContainerResource
    {
        if (builder.ApplicationBuilder.ExecutionContext.IsRunMode)
        {
            builder.WithImageSHA256(sha256);
        }

        return builder;
    }
}
