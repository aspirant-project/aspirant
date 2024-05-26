using Aspire.Hosting;
using Aspire.Hosting.ApplicationModel;

namespace Aspirant.Hosting;

/// <summary>
/// <c>PublishWithEnvironment</c> Extension methods for <see cref="IResourceBuilder{TResource}"/>.
/// </summary>
public static class ResourceBuilderPublishWithEnvironmentExtensions
{
    /// <summary>
    /// Allows for the population of environment variables on a resource when <see cref="DistributedApplicationExecutionContext.IsPublishMode"/> is <c>true</c>.
    /// </summary>
    /// <typeparam name="TResource">The resource type.</typeparam>
    /// <param name="builder">The resource builder.</param>
    /// <param name="callback">The callback.</param>
    /// <returns>The resource builder.</returns>
    public static IResourceBuilder<TResource> PublishWithEnvironment<TResource>(this IResourceBuilder<TResource> builder, Action<EnvironmentCallbackContext> callback)
        where TResource : IResourceWithEnvironment
    {
        if (builder.ApplicationBuilder.ExecutionContext.IsPublishMode)
        {
            builder.WithEnvironment(callback);
        }

        return builder;
    }

    /// <summary>
    /// Allows for the population of environment variables on a resource when <see cref="DistributedApplicationExecutionContext.IsPublishMode"/> is <c>true</c>.
    /// </summary>
    /// <typeparam name="TResource">The resource type.</typeparam>
    /// <param name="builder">The resource builder.</param>
    /// <param name="callback">The callback.</param>
    /// <returns>The resource builder.</returns>
    public static IResourceBuilder<TResource> PublishWithEnvironment<TResource>(this IResourceBuilder<TResource> builder, Func<EnvironmentCallbackContext, Task> callback)
        where TResource : IResourceWithEnvironment
    {
        if (builder.ApplicationBuilder.ExecutionContext.IsPublishMode)
        {
            builder.WithEnvironment(callback);
        }

        return builder;
    }

    /// <summary>
    /// Adds an environment variable to the resource with the endpoint for <paramref name="endpointReference"/> when <see cref="DistributedApplicationExecutionContext.IsPublishMode"/> is <c>true</c>.
    /// </summary>
    /// <typeparam name="TResource">The resource type.</typeparam>
    /// <param name="builder">The resource builder.</param>
    /// <param name="name">The environment variable name.</param>
    /// <param name="endpointReference">The endpoint reference.</param>
    /// <returns>The resource builder.</returns>
    public static IResourceBuilder<TResource> PublishWithEnvironment<TResource>(this IResourceBuilder<TResource> builder, string name, EndpointReference endpointReference)
        where TResource : IResourceWithEnvironment
    {
        if (builder.ApplicationBuilder.ExecutionContext.IsPublishMode)
        {
            builder.WithEnvironment(name, endpointReference);
        }

        return builder;
    }

    /// <summary>
    /// Adds an environment variable to the resource when <see cref="DistributedApplicationExecutionContext.IsPublishMode"/> is <c>true</c>.
    /// </summary>
    /// <typeparam name="TResource">The resource type.</typeparam>
    /// <param name="builder">The resource builder.</param>
    /// <param name="name">The environment variable name.</param>
    /// <param name="value">The value of the environment variable.</param>
    /// <returns>The resource builder.</returns>
    public static IResourceBuilder<TResource> PublishWithEnvironment<TResource>(this IResourceBuilder<TResource> builder, string name, ReferenceExpression value)
        where TResource : IResourceWithEnvironment
    {
        if (builder.ApplicationBuilder.ExecutionContext.IsPublishMode)
        {
            builder.WithEnvironment(name, value);
        }

        return builder;
    }

    /// <summary>
    /// Adds an environment variable to the resource when <see cref="DistributedApplicationExecutionContext.IsPublishMode"/> is <c>true</c>.
    /// </summary>
    /// <typeparam name="TResource">The resource type.</typeparam>
    /// <param name="builder">The resource builder.</param>
    /// <param name="name">The environment variable name.</param>
    /// <param name="value">The value of the environment variable.</param>
    /// <returns>The resource builder.</returns>
    public static IResourceBuilder<TResource> PublishWithEnvironment<TResource>(this IResourceBuilder<TResource> builder, string name, in ReferenceExpression.ExpressionInterpolatedStringHandler value)
        where TResource : IResourceWithEnvironment
    {
        if (builder.ApplicationBuilder.ExecutionContext.IsPublishMode)
        {
            builder.WithEnvironment(name, value);
        }

        return builder;
    }

    /// <summary>
    /// Adds an environment variable to the resource when <see cref="DistributedApplicationExecutionContext.IsPublishMode"/> is <c>true</c>.
    /// </summary>
    /// <typeparam name="TResource">The resource type.</typeparam>
    /// <param name="builder">The resource builder.</param>
    /// <param name="name">The environment variable name.</param>
    /// <param name="value">The value of the environment variable.</param>
    /// <returns>The resource builder.</returns>
    public static IResourceBuilder<TResource> PublishWithEnvironment<TResource>(this IResourceBuilder<TResource> builder, string name, string? value)
        where TResource : IResourceWithEnvironment
    {
        if (builder.ApplicationBuilder.ExecutionContext.IsPublishMode)
        {
            builder.WithEnvironment(name, value);
        }

        return builder;
    }

    /// <summary>
    /// Adds an environment variable to the resource when <see cref="DistributedApplicationExecutionContext.IsPublishMode"/> is <c>true</c>.
    /// </summary>
    /// <typeparam name="TResource">The resource type.</typeparam>
    /// <param name="builder">The resource builder.</param>
    /// <param name="name">The environment variable name.</param>
    /// <param name="callback">
    /// A callback that allows for deferred execution of a specific environment variable.
    /// This runs after resources have been allocated by the orchestrator and allows
    /// access to other resources to resolve computed data, e.g. connection strings,
    /// ports.
    /// </param>
    /// <returns>The resource builder.</returns>
    public static IResourceBuilder<TResource> PublishWithEnvironment<TResource>(this IResourceBuilder<TResource> builder, string name, Func<string> callback)
        where TResource : IResourceWithEnvironment
    {
        if (builder.ApplicationBuilder.ExecutionContext.IsPublishMode)
        {
            builder.WithEnvironment(name, callback);
        }

        return builder;
    }

    /// <summary>
    /// Adds an environment variable to the resource with the connection string from the referenced resource when <see cref="DistributedApplicationExecutionContext.IsPublishMode"/> is <c>true</c>.
    /// </summary>
    /// <typeparam name="TResource">The resource type.</typeparam>
    /// <param name="builder">The resource builder.</param>
    /// <param name="envVarName">The name of the environment variable under which the connection string will be set.</param>
    /// <param name="resource">The resource builder of the referenced service from which to pull the connection string.</param>
    /// <returns>The resource builder.</returns>
    public static IResourceBuilder<TResource> PublishWithEnvironment<TResource>(this IResourceBuilder<TResource> builder, string envVarName, IResourceBuilder<IResourceWithConnectionString> resource)
        where TResource : IResourceWithEnvironment
    {
        if (builder.ApplicationBuilder.ExecutionContext.IsPublishMode)
        {
            builder.WithEnvironment(envVarName, resource);
        }

        return builder;
    }

    /// <summary>
    /// Adds an environment variable to the resource with the value from parameter when <see cref="DistributedApplicationExecutionContext.IsPublishMode"/> is <c>true</c>.
    /// </summary>
    /// <typeparam name="TResource">The resource type.</typeparam>
    /// <param name="builder">The resource builder.</param>
    /// <param name="name">The name of the environment variable.</param>
    /// <param name="parameter">Resource builder for the parameter resource..</param>
    /// <returns>The resource builder.</returns>
    public static IResourceBuilder<TResource> PublishWithEnvironment<TResource>(this IResourceBuilder<TResource> builder, string name, IResourceBuilder<ParameterResource> parameter)
        where TResource : IResourceWithEnvironment
    {
        if (builder.ApplicationBuilder.ExecutionContext.IsPublishMode)
        {
            builder.WithEnvironment(name, parameter);
        }

        return builder;
    }
}
