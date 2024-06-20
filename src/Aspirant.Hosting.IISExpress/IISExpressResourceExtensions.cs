using Aspire.Hosting;
using Aspire.Hosting.ApplicationModel;
// ReSharper disable InconsistentNaming
// ReSharper disable CheckNamespace

namespace Aspirant.Hosting;

/// <summary>
/// Extensions for the <see cref="IISExpressResource"/>
/// </summary>
public static class IISExpressResourceExtensions
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="builder">The builder.</param>
    /// <param name="name">The name of the resource.</param>
    /// <param name="applicationPath">The path to the web application to host in IIS Express.</param>
    /// <param name="arch">The architecture of IIS Express to be used.</param>
    /// <returns>The builder.</returns>
    public static IResourceBuilder<IISExpressResource> AddIISExpress(this IDistributedApplicationBuilder builder, string name, string applicationPath)
    {
        var iisExpressPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86), "IIS Express", "iisexpress.exe");

        var appPath = Path.GetFullPath(applicationPath, builder.AppHostDirectory);

        var resource = new IISExpressResource(name, iisExpressPath);

        return builder.AddResource(resource)
            .WithArgs($"/path:{appPath}", "/systray:false")
            .WithArgs(context =>
            {
                var http = resource.GetEndpoint("http");
                
                var portExpression = ReferenceExpression.Create($"/port:{http.Property(EndpointProperty.TargetPort)}");
                context.Args.Add(portExpression);
            })
            .WithHttpEndpoint(name: "http");
    }
}
