using Aspire.Hosting;
using Aspire.Hosting.ApplicationModel;
using Microsoft.Extensions.DependencyInjection;

namespace Aspirant.Hosting.UnitTests;

public class RunWithTests
{
    [Fact]
    public void AnnotationAddedWhenRunning()
    {
        var builder = DistributedApplication.CreateBuilder();
        var runWithAnnotation = new TestAnnotation();
        var resource = builder.AddResource(new TestResource("resourceA"))
            .WithAnnotation<TestAnnotation>()
            .RunWithAnnotation(runWithAnnotation);

        using var app = builder.Build();
        var appModel = app.Services.GetRequiredService<DistributedApplicationModel>();
        var resourceA = appModel.Resources.First(p => p.Name == "resourceA");

        var annotations = resourceA.Annotations.OfType<TestAnnotation>();
        Assert.Contains(runWithAnnotation, annotations);
    }

    [Fact]
    public void AnnotationNotAddedWhenPublishing()
    {
        var builder = DistributedApplication.CreateBuilder(["Publishing:Publisher=manifest"]);
        var runWithAnnotation = new TestAnnotation();
        var resource = builder.AddResource(new TestResource("resourceA"))
            .WithAnnotation<TestAnnotation>()
            .RunWithAnnotation(runWithAnnotation);

        using var app = builder.Build();
        var appModel = app.Services.GetRequiredService<DistributedApplicationModel>();
        var resourceA = appModel.Resources.First(p => p.Name == "resourceA");

        var annotations = resourceA.Annotations.OfType<TestAnnotation>();
        Assert.DoesNotContain(runWithAnnotation, annotations);
    }

    [Fact]
    public void SimpleEnvironmentWithNameAndValueAddedWhenRunning()
    {
        var builder = DistributedApplication.CreateBuilder();
        var resource = builder.AddResource(new TestResource("resourceA"))
            .WithEnvironment("first", "firstValue")
            .RunWithEnvironment("myName", "value");

        using var app = builder.Build();
        var appModel = app.Services.GetRequiredService<DistributedApplicationModel>();
        var resourceA = appModel.Resources.First(p => p.Name == "resourceA");

        var environmentVariables = MaterializeEnvironmentVariables(app, resourceA);
        Assert.Equal("value", environmentVariables["myName"]);
    }

    [Fact]
    public void SimpleEnvironmentWithNameAndValueNotAddedWhenPublishing()
    {
        var builder = DistributedApplication.CreateBuilder(["Publishing:Publisher=manifest"]);
        var resource = builder.AddResource(new TestResource("resourceA"))
            .WithEnvironment("first", "firstValue")
            .RunWithEnvironment("myName", "value");

        using var app = builder.Build();
        var appModel = app.Services.GetRequiredService<DistributedApplicationModel>();
        var resourceA = appModel.Resources.First(p => p.Name == "resourceA");

        var environmentVariables = MaterializeEnvironmentVariables(app, resourceA);
        Assert.DoesNotContain("myName", environmentVariables.Keys);
    }

    private static Dictionary<string, object> MaterializeEnvironmentVariables(DistributedApplication app, IResource resource)
    {
        Dictionary<string, object> environmentVariables = [];

        if (resource.TryGetEnvironmentVariables(out var annotations))
        {
            var executionContext = app.Services.GetRequiredService<DistributedApplicationExecutionContext>();
            var context = new EnvironmentCallbackContext(executionContext, environmentVariables);
            foreach (var callbackAnnotation in annotations)
            {
                callbackAnnotation.Callback(context);
            }
        }

        return environmentVariables;
    }

    private sealed class TestResource(string name) : IResourceWithEnvironment
    {
        public string Name => name;

        public ResourceAnnotationCollection Annotations { get; } = [];
    }

    private sealed class TestAnnotation : IResourceAnnotation
    {
        
    }
}
