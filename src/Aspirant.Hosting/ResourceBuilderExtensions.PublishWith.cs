using Aspire.Hosting;
using Aspire.Hosting.ApplicationModel;

namespace Aspirant.Hosting;

/// <summary>
/// Extension methods for <see cref="IResourceBuilder{TResource}"/>.
/// </summary>
public static partial class ResourceBuilderExtensions
{
/// <summary>
    /// Adds an annotation to the resource being built when <see cref="DistributedApplicationExecutionContext.IsPublishMode"/> is <c>true</c>.
    /// </summary>
    /// <typeparam name="TResource">The resource type.</typeparam>
    /// <typeparam name="TAnnotation">The annotation type.</typeparam>
    /// <param name="builder">The resource builder.</param>
    /// <param name="behavior">The behavior to use when adding the annotation.</param>
    /// <returns>The resource builder.</returns>
    public static IResourceBuilder<TResource> PublishWithAnnotation<TResource, TAnnotation>(this IResourceBuilder<TResource> builder, ResourceAnnotationMutationBehavior behavior = ResourceAnnotationMutationBehavior.Append)
        where TResource : IResource
        where TAnnotation : IResourceAnnotation, new()
    {
        if (builder.ApplicationBuilder.ExecutionContext.IsPublishMode)
        {
            builder.WithAnnotation<TAnnotation>(behavior);
        }

        return builder;
    }

    /// <summary>
    /// Adds an annotation to the resource being built when <see cref="DistributedApplicationExecutionContext.IsPublishMode"/> is <c>true</c>.
    /// </summary>
    /// <typeparam name="TResource">The resource type.</typeparam>
    /// <typeparam name="TAnnotation">The annotation type.</typeparam>
    /// <param name="builder">The resource builder.</param>
    /// <param name="annotation">The annotation to add.</param>
    /// <param name="behavior">The behavior to use when adding the annotation.</param>
    /// <returns>The resource builder.</returns>
    public static IResourceBuilder<TResource> PublishWithAnnotation<TResource, TAnnotation>(this IResourceBuilder<TResource> builder, TAnnotation annotation, ResourceAnnotationMutationBehavior behavior = ResourceAnnotationMutationBehavior.Append)
        where TResource : IResource
        where TAnnotation : IResourceAnnotation, new()
    {
        if (builder.ApplicationBuilder.ExecutionContext.IsPublishMode)
        {
            builder.WithAnnotation(annotation, behavior);
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

    /// <summary>
    /// Injects service discovery information as environment variables from the project resource into the destination resource when
    /// <see cref="DistributedApplicationExecutionContext.IsPublishMode"/> is <c>true</c>, using the source resource's name as the service name.
    /// Each endpoint defined on the project resource will be injected using the format "services__{sourceResourceName}__{endpointName}__{endpointIndex}={uriString}."
    /// </summary>
    /// <typeparam name="TDestination">The destination resource.</typeparam>
    /// <param name="builder">The resource where the service discovery information will be injected.</param>
    /// <param name="source">The resource from which to extract service discovery information.</param>
    /// <returns>A reference to the <see cref="IResourceBuilder{T}"/>.</returns>
    public static IResourceBuilder<TDestination> PublishWithReference<TDestination>(this IResourceBuilder<TDestination> builder, IResourceBuilder<IResourceWithServiceDiscovery> source)
        where TDestination : IResourceWithEnvironment
    {
        if (builder.ApplicationBuilder.ExecutionContext.IsPublishMode)
        {
            builder.WithReference(source);
        }

        return builder;
    }

    /// <summary>
    /// Injects service discovery information as environment variables from the uri into the destination resource when
    /// <see cref="DistributedApplicationExecutionContext.IsPublishMode"/> is <c>true</c>, using the name as the service name.
    /// The uri will be injected using the format "services__{name}__default__0={uri}."
    /// </summary>
    /// <typeparam name="TDestination"></typeparam>
    /// <param name="builder">The resource where the service discovery information will be injected.</param>
    /// <param name="name">The name of the service.</param>
    /// <param name="uri">The uri of the service.</param>
    /// <returns>A reference to the <see cref="IResourceBuilder{T}"/>.</returns>
    public static IResourceBuilder<TDestination> PublishWithReference<TDestination>(this IResourceBuilder<TDestination> builder, string name, Uri uri)
        where TDestination : IResourceWithEnvironment
    {
        if (builder.ApplicationBuilder.ExecutionContext.IsPublishMode)
        {
            builder.WithReference(name, uri);
        }

        return builder;
    }

    /// <summary>
    /// Injects service discovery information from the specified endpoint into the project resource when
    /// <see cref="DistributedApplicationExecutionContext.IsPublishMode"/> is <c>true</c>, using the source resource's name as the service name.
    /// Each endpoint will be injected using the format "services__{sourceResourceName}__{endpointName}__{endpointIndex}={uriString}."
    /// </summary>
    /// <typeparam name="TDestination">The destination resource.</typeparam>
    /// <param name="builder">The resource where the service discovery information will be injected.</param>
    /// <param name="endpointReference">The endpoint from which to extract the url.</param>
    /// <returns>A reference to the <see cref="IResourceBuilder{T}"/>.</returns>
    public static IResourceBuilder<TDestination> PublishWithReference<TDestination>(this IResourceBuilder<TDestination> builder, EndpointReference endpointReference)
        where TDestination : IResourceWithEnvironment
    {
        if (builder.ApplicationBuilder.ExecutionContext.IsPublishMode)
        {
            builder.WithReference(endpointReference);
        }

        return builder;
    }

    /// <summary>
    /// Changes an existing or creates a new endpoint if it doesn't exist and invokes callback to modify the defaults when
    /// <see cref="DistributedApplicationExecutionContext.IsPublishMode"/> is <c>true</c>.
    /// </summary>
    /// <param name="builder">Resource builder for resource with endpoints.</param>
    /// <param name="endpointName">Name of endpoint to change.</param>
    /// <param name="callback">Callback that modifies the endpoint.</param>
    /// <param name="createIfNotExists">Create endpoint if it does not exist.</param>
    /// <returns></returns>
    public static IResourceBuilder<T> PublishWithEndpoint<T>(this IResourceBuilder<T> builder, string endpointName, Action<EndpointAnnotation> callback, bool createIfNotExists = true) where T : IResourceWithEndpoints
    {
        if (builder.ApplicationBuilder.ExecutionContext.IsPublishMode)
        {
            builder.WithEndpoint(endpointName, callback, createIfNotExists);
        }

        return builder;
    }

    /// <summary>
    /// Exposes an endpoint on a resource when <see cref="DistributedApplicationExecutionContext.IsPublishMode"/> is <c>true</c>.<br/>
    /// This endpoint reference can be retrieved using <see cref="Aspire.Hosting.ResourceBuilderExtensions.GetEndpoint{T}(IResourceBuilder{T}, string)"/>.
    /// The endpoint name will be the scheme name if not specified.
    /// </summary>
    /// <typeparam name="T">The resource type.</typeparam>
    /// <param name="builder">The resource builder.</param>
    /// <param name="targetPort">This is the port the resource is listening on. If the endpoint is used for the container, it is the container port.</param>
    /// <param name="port">An optional port. This is the port that will be given to other resources to communicate with this resource.</param>
    /// <param name="scheme">An optional scheme e.g. (http/https). Defaults to "tcp" if not specified.</param>
    /// <param name="name">An optional name of the endpoint. Defaults to the scheme name if not specified.</param>
    /// <param name="env">An optional name of the environment variable that will be used to inject the <paramref name="targetPort"/>. If the target port is null one will be dynamically generated and assigned to the environment variable.</param>
    /// <param name="isExternal">Indicates that this endpoint should be exposed externally at publish time.</param>
    /// <param name="isProxied">Specifies if the endpoint will be proxied by DCP. Defaults to true.</param>
    /// <returns>The <see cref="IResourceBuilder{T}"/>.</returns>
    /// <exception cref="DistributedApplicationException">Throws an exception if an endpoint with the same name already exists on the specified resource.</exception>
    public static IResourceBuilder<T> PublishWithEndpoint<T>(this IResourceBuilder<T> builder, int? port = null, int? targetPort = null, string? scheme = null, string? name = null, string? env = null, bool isProxied = true, bool? isExternal = null) where T : IResource
    {
        if (builder.ApplicationBuilder.ExecutionContext.IsPublishMode)
        {
            builder.WithEndpoint(port, targetPort, scheme, name, env, isProxied, isExternal);
        }

        return builder;
    }

    /// <summary>
    /// Exposes an HTTP endpoint on a resource when <see cref="DistributedApplicationExecutionContext.IsPublishMode"/> is <c>true</c>.<br/>
    /// This endpoint reference can be retrieved using <see cref="Aspire.Hosting.ResourceBuilderExtensions.GetEndpoint{T}(IResourceBuilder{T}, string)"/>.
    /// The endpoint name will be "http" if not specified.
    /// </summary>
    /// <typeparam name="T">The resource type.</typeparam>
    /// <param name="builder">The resource builder.</param>
    /// <param name="targetPort">This is the port the resource is listening on. If the endpoint is used for the container, it is the container port.</param>
    /// <param name="port">An optional port. This is the port that will be given to other resources to communicate with this resource.</param>
    /// <param name="name">An optional name of the endpoint. Defaults to "http" if not specified.</param>
    /// <param name="env">An optional name of the environment variable to inject.</param>
    /// <param name="isProxied">Specifies if the endpoint will be proxied by DCP. Defaults to true.</param>
    /// <returns>The <see cref="IResourceBuilder{T}"/>.</returns>
    /// <exception cref="DistributedApplicationException">Throws an exception if an endpoint with the same name already exists on the specified resource.</exception>
    public static IResourceBuilder<T> PublishWithHttpEndpoint<T>(this IResourceBuilder<T> builder, int? port = null, int? targetPort = null, string? name = null, string? env = null, bool isProxied = true) where T : IResource
    {
        if (builder.ApplicationBuilder.ExecutionContext.IsPublishMode)
        {
            builder.WithHttpEndpoint(port, targetPort, name, env, isProxied);
        }

        return builder;
    }

    /// <summary>
    /// Exposes an HTTPS endpoint on a resource when <see cref="DistributedApplicationExecutionContext.IsPublishMode"/> is <c>true</c>.<br/>
    /// This endpoint reference can be retrieved using <see cref="Aspire.Hosting.ResourceBuilderExtensions.GetEndpoint{T}(IResourceBuilder{T}, string)"/>.
    /// The endpoint name will be "https" if not specified.
    /// </summary>
    /// <typeparam name="T">The resource type.</typeparam>
    /// <param name="builder">The resource builder.</param>
    /// <param name="targetPort">This is the port the resource is listening on. If the endpoint is used for the container, it is the container port.</param>
    /// <param name="port">An optional host port.</param>
    /// <param name="name">An optional name of the endpoint. Defaults to "https" if not specified.</param>
    /// <param name="env">An optional name of the environment variable to inject.</param>
    /// <param name="isProxied">Specifies if the endpoint will be proxied by DCP. Defaults to true.</param>
    /// <returns>The <see cref="IResourceBuilder{T}"/>.</returns>
    /// <exception cref="DistributedApplicationException">Throws an exception if an endpoint with the same name already exists on the specified resource.</exception>
    public static IResourceBuilder<T> PublishWithHttpsEndpoint<T>(this IResourceBuilder<T> builder, int? port = null, int? targetPort = null, string? name = null, string? env = null, bool isProxied = true) where T : IResource
    {
        if (builder.ApplicationBuilder.ExecutionContext.IsPublishMode)
        {
            builder.WithHttpsEndpoint(port, targetPort, name, env, isProxied);
        }

        return builder;
    }

    /// <summary>
    /// Marks existing http or https endpoints on a resource as external when <see cref="DistributedApplicationExecutionContext.IsPublishMode"/> is <c>true</c>.
    /// </summary>
    /// <typeparam name="T">The resource type.</typeparam>
    /// <param name="builder">The resource builder.</param>
    /// <returns></returns>
    public static IResourceBuilder<T> PublishWithExternalHttpEndpoints<T>(this IResourceBuilder<T> builder) where T : IResourceWithEndpoints
    {
        if (builder.ApplicationBuilder.ExecutionContext.IsPublishMode)
        {
            builder.WithExternalHttpEndpoints();
        }

        return builder;
    }

    /// <summary>
    /// Configures a resource to mark all endpoints' transport as HTTP/2 when <see cref="DistributedApplicationExecutionContext.IsPublishMode"/> is <c>true</c>.
    /// This is useful for HTTP/2 services that need prior knowledge.
    /// </summary>
    /// <typeparam name="T">The resource type.</typeparam>
    /// <param name="builder">The resource builder.</param>
    /// <returns>The <see cref="IResourceBuilder{T}"/>.</returns>
    public static IResourceBuilder<T> PublishAsHttp2Service<T>(this IResourceBuilder<T> builder) where T : IResourceWithEndpoints
    {
        if (builder.ApplicationBuilder.ExecutionContext.IsPublishMode)
        {
            builder.AsHttp2Service();
        }

        return builder;
    }
}
