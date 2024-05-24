using Aspire.Hosting.ApplicationModel;

namespace Aspirant.Hosting.Testing;

internal static class ResourceExtensions
{
    /// <summary>
    /// Gets the name of the <see cref="ProjectResource"/> based on the project file path.
    /// </summary>
    public static string GetName(this ProjectResource project)
    {
        var metadata = project.GetProjectMetadata();
        return Path.GetFileNameWithoutExtension(metadata.ProjectPath);
    }
}
