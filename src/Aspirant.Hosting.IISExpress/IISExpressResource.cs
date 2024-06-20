using Aspire.Hosting;
using Aspire.Hosting.ApplicationModel;
// ReSharper disable InconsistentNaming
// ReSharper disable CheckNamespace

namespace Aspirant.Hosting;

/// <summary>
/// Represents a IIS Express resource.
/// </summary>
/// <param name="name">The name of the resource in the application model.</param>
/// <param name="path">The path to the IIS Express executable.</param>
/// <param name="workingDirectory">The working directory of the executable.</param>
public class IISExpressResource(string name, string path, string workingDirectory = ".")
    : ExecutableResource(name, path, workingDirectory), IResourceWithServiceDiscovery
{
    
}