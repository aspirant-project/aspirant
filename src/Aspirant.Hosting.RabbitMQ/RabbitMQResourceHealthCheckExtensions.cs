﻿using Aspire.Hosting.ApplicationModel;
using HealthChecks.RabbitMQ;

namespace Aspirant.Hosting.RabbitMQ;

/// <summary>
/// Adds a health check to RabbitMQ resources.
/// </summary>
public static class RabbitMQResourceHealthCheckExtensions
{
    /// <summary>
    /// Adds a health check to the RabbitMQ server resource.
    /// </summary>
    public static IResourceBuilder<RabbitMQServerResource> WithHealthCheck(this IResourceBuilder<RabbitMQServerResource> builder)
    {
        return builder.WithAnnotation(HealthCheckAnnotation.Create(cs => new RabbitMQHealthCheck(new RabbitMQHealthCheckOptions { ConnectionUri = new(cs) })));
    }
}