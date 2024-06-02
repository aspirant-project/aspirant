using Aspire.Hosting;
using Aspire.Hosting.ApplicationModel;
using Aspire.Hosting.Redis;

namespace Aspirant.Hosting;

public static partial class RedisResourceBuilderExtensions
{
    /// <summary>
    /// Configures the host port that the Redis Commander resource is exposed on instead of using randomly assigned port when
    /// <see cref="DistributedApplicationExecutionContext.IsPublishMode"/> is <c>true</c>..
    /// </summary>
    /// <param name="builder">The resource builder for Redis Commander.</param>
    /// <param name="port">The port to bind on the host. If <see langword="null"/> is used random port will be assigned.</param>
    /// <returns>The resource builder for PGAdmin.</returns>
    public static IResourceBuilder<RedisCommanderResource> PublishWithHostPort(this IResourceBuilder<RedisCommanderResource> builder, int? port)
    {
        if (builder.ApplicationBuilder.ExecutionContext.IsPublishMode)
        {
            builder.WithHostPort(port);
        }

        return builder;
    }

    /// <summary>
    /// Adds a named volume for the data folder to a Redis container resource and enables Redis persistence when
    /// <see cref="DistributedApplicationExecutionContext.IsPublishMode"/> is <c>true</c>.
    /// </summary>
    /// <remarks>
    /// Use <see cref="PublishWithPersistence(IResourceBuilder{RedisResource}, TimeSpan?, long)"/> to adjust Redis persistence configuration, e.g.:
    /// <code>
    /// var cache = builder.AddRedis("cache")
    ///                    .PublishWithDataVolume()
    ///                    .PublishWithPersistence(TimeSpan.FromSeconds(10), 5);
    /// </code>
    /// </remarks>
    /// <param name="builder">The resource builder.</param>
    /// <param name="name">The name of the volume. Defaults to an auto-generated name based on the application and resource names.</param>
    /// <param name="isReadOnly">
    /// A flag that indicates if this is a read-only volume. Setting this to <c>true</c> will disable Redis persistence.<br/>
    /// Defaults to <c>false</c>.
    /// </param>
    /// <returns>The <see cref="IResourceBuilder{T}"/>.</returns>
    public static IResourceBuilder<RedisResource> PublishWithDataVolume(this IResourceBuilder<RedisResource> builder, string? name = null, bool isReadOnly = false)
    {
        if (builder.ApplicationBuilder.ExecutionContext.IsPublishMode)
        {
            builder.WithDataVolume(name, isReadOnly);
        }

        return builder;
    }

    /// <summary>
    /// Adds a bind mount for the data folder to a Redis container resource and enables Redis persistence
    /// when <see cref="DistributedApplicationExecutionContext.IsPublishMode"/> is <c>true</c>.
    /// </summary>
    /// <remarks>
    /// Use <see cref="PublishWithPersistence(IResourceBuilder{RedisResource}, TimeSpan?, long)"/> to adjust Redis persistence configuration, e.g.:
    /// <code>
    /// var cache = builder.AddRedis("cache")
    ///                    .PublishWithDataBindMount()
    ///                    .PublishWithPersistence(TimeSpan.FromSeconds(10), 5);
    /// </code>
    /// </remarks>
    /// <param name="builder">The resource builder.</param>
    /// <param name="source">The source directory on the host to mount into the container.</param>
    /// <param name="isReadOnly">
    /// A flag that indicates if this is a read-only mount. Setting this to <c>true</c> will disable Redis persistence.<br/>
    /// Defaults to <c>false</c>.
    /// </param>
    /// <returns>The <see cref="IResourceBuilder{T}"/>.</returns>
    public static IResourceBuilder<RedisResource> PublishWithDataBindMount(this IResourceBuilder<RedisResource> builder, string source, bool isReadOnly = false)
    {
        if (builder.ApplicationBuilder.ExecutionContext.IsPublishMode)
        {
            builder.WithDataBindMount(source, isReadOnly);
        }

        return builder;
    }

    /// <summary>
    /// Configures a Redis container resource for persistence when <see cref="DistributedApplicationExecutionContext.IsPublishMode"/> is <c>true</c>.
    /// </summary>
    /// <remarks>
    /// Use with <see cref="PublishWithDataBindMount(IResourceBuilder{RedisResource}, string, bool)"/>
    /// or <see cref="PublishWithDataVolume(IResourceBuilder{RedisResource}, string?, bool)"/> to persist Redis data across sessions with custom persistence configuration, e.g.:
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
    public static IResourceBuilder<RedisResource> PublishWithPersistence(this IResourceBuilder<RedisResource> builder, TimeSpan? interval = null, long keysChangedThreshold = 1)
    {
        if (builder.ApplicationBuilder.ExecutionContext.IsPublishMode)
        {
            builder.WithPersistence(interval, keysChangedThreshold);
        }

        return builder;
    }
}
