using System.Xml.Linq;
using Aspire.Hosting;
using Aspire.Hosting.ApplicationModel;
using Aspire.Hosting.Redis;

namespace Aspirant.Hosting;

/// <summary>
/// Extension methods for <see cref="IResourceBuilder{RedisResource}"/>.
/// </summary>
public static partial class RedisResourceBuilderExtensions
{
    /// <summary>
    /// Configures the host port that the Redis Commander resource is exposed on instead of using randomly assigned port when
    /// <see cref="DistributedApplicationExecutionContext.IsRunMode"/> is <c>true</c>..
    /// </summary>
    /// <param name="builder">The resource builder for Redis Commander.</param>
    /// <param name="port">The port to bind on the host. If <see langword="null"/> is used random port will be assigned.</param>
    /// <returns>The resource builder for PGAdmin.</returns>
    public static IResourceBuilder<RedisCommanderResource> RunWithHostPort(this IResourceBuilder<RedisCommanderResource> builder, int? port)
    {
        if (builder.ApplicationBuilder.ExecutionContext.IsRunMode)
        {
            builder.WithHostPort(port);
        }

        return builder;
    }

    /// <summary>
    /// Adds a named volume for the data folder to a Redis container resource and enables Redis persistence when
    /// <see cref="DistributedApplicationExecutionContext.IsRunMode"/> is <c>true</c>.
    /// </summary>
    /// <remarks>
    /// Use <see cref="RunWithPersistence(IResourceBuilder{RedisResource}, TimeSpan?, long)"/> to adjust Redis persistence configuration, e.g.:
    /// <code>
    /// var cache = builder.AddRedis("cache")
    ///                    .RunWithDataVolume()
    ///                    .RunWithPersistence(TimeSpan.FromSeconds(10), 5);
    /// </code>
    /// </remarks>
    /// <param name="builder">The resource builder.</param>
    /// <param name="name">The name of the volume. Defaults to an auto-generated name based on the application and resource names.</param>
    /// <param name="isReadOnly">
    /// A flag that indicates if this is a read-only volume. Setting this to <c>true</c> will disable Redis persistence.<br/>
    /// Defaults to <c>false</c>.
    /// </param>
    /// <returns>The <see cref="IResourceBuilder{T}"/>.</returns>
    public static IResourceBuilder<RedisResource> RunWithDataVolume(this IResourceBuilder<RedisResource> builder, string? name = null, bool isReadOnly = false)
    {
        if (builder.ApplicationBuilder.ExecutionContext.IsRunMode)
        {
            builder.WithDataVolume(name, isReadOnly);
        }

        return builder;
    }

    /// <summary>
    /// Adds a bind mount for the data folder to a Redis container resource and enables Redis persistence
    /// when <see cref="DistributedApplicationExecutionContext.IsRunMode"/> is <c>true</c>.
    /// </summary>
    /// <remarks>
    /// Use <see cref="RunWithPersistence(IResourceBuilder{RedisResource}, TimeSpan?, long)"/> to adjust Redis persistence configuration, e.g.:
    /// <code>
    /// var cache = builder.AddRedis("cache")
    ///                    .RunWithDataBindMount()
    ///                    .RunWithPersistence(TimeSpan.FromSeconds(10), 5);
    /// </code>
    /// </remarks>
    /// <param name="builder">The resource builder.</param>
    /// <param name="source">The source directory on the host to mount into the container.</param>
    /// <param name="isReadOnly">
    /// A flag that indicates if this is a read-only mount. Setting this to <c>true</c> will disable Redis persistence.<br/>
    /// Defaults to <c>false</c>.
    /// </param>
    /// <returns>The <see cref="IResourceBuilder{T}"/>.</returns>
    public static IResourceBuilder<RedisResource> RunWithDataBindMount(this IResourceBuilder<RedisResource> builder, string source, bool isReadOnly = false)
    {
        if (builder.ApplicationBuilder.ExecutionContext.IsRunMode)
        {
            builder.WithDataBindMount(source, isReadOnly);
        }

        return builder;
    }

    /// <summary>
    /// Configures a Redis container resource for persistence when <see cref="DistributedApplicationExecutionContext.IsRunMode"/> is <c>true</c>.
    /// </summary>
    /// <remarks>
    /// Use with <see cref="RunWithDataBindMount(IResourceBuilder{RedisResource}, string, bool)"/>
    /// or <see cref="RunWithDataVolume(IResourceBuilder{RedisResource}, string?, bool)"/> to persist Redis data across sessions with custom persistence configuration, e.g.:
    /// <code>
    /// var cache = builder.AddRedis("cache")
    ///                    .WithDataVolume()
    ///                    .WithPersistence(TimeSpan.FromSeconds(10), 5);
    /// </code>
    /// </remarks>
    /// <param name="builder">The resource builder.</param>
    /// <param name="interval">The interval between snapshot exports. Defaults to 60 seconds.</param>
    /// <param name="keysChangedThreshold">The number of key change operations required to trigger a snapshot at the interval. Defaults to 1.</param>
    /// <returns>The <see cref="IResourceBuilder{T}"/>.</returns>
    public static IResourceBuilder<RedisResource> RunWithPersistence(this IResourceBuilder<RedisResource> builder, TimeSpan? interval = null, long keysChangedThreshold = 1)
    {
        if (builder.ApplicationBuilder.ExecutionContext.IsRunMode)
        {
            builder.WithPersistence(interval, keysChangedThreshold);
        }

        return builder;
    }
}
