using Aspire.Hosting.ApplicationModel;
using HealthChecks.Redis;

namespace Aspirant.Hosting;

/// <summary>
/// Adds a health check to Redis resources.
/// </summary>
public static class RedisResourceHealthCheckExtensions
{
    /// <summary>
    /// Adds a health check to the Redis server resource.
    /// </summary>
    public static IResourceBuilder<RedisResource> WithHealthCheck(this IResourceBuilder<RedisResource> builder)
    {
        return builder.WithAnnotation(HealthCheckAnnotation.Create(cs => new RedisHealthCheck(cs)));
    }
}
