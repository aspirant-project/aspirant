using System.Net;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Aspirant.Hosting.Testing;
using Xunit.Abstractions;

namespace SamplesTests.Tests;

public class YarpResourceSample(ITestOutputHelper testOutputHelper)
{
    [Theory]
    [InlineData(null)]
    [InlineData(8001)]
    public async Task GetAppsThroughYarpIngressResourceReturnsOkStatusCode(int? ingressPort)
    {
        // Arrange
        var appHost = await DistributedApplicationTestingBuilder.CreateAsync<Projects.YarpResource_AppHost>();
        appHost.FixContentRoot();
        appHost.WriteOutputTo(testOutputHelper);
        if (ingressPort is not null)
        {
            appHost.Configuration.AddInMemoryCollection(new Dictionary<string, string?> { { "Ingress:Port", ingressPort.ToString() } });
        }
        appHost.Services.ConfigureHttpClientDefaults(options =>
        {
            options.AddStandardResilienceHandler();
        });
        
        await using var app = await appHost.BuildAsync();
        await app.StartAsync(waitForResourcesToStart: true);

        // Act/Assert
        var httpClient = app.CreateHttpClient("ingress");

        var targets = new List<(string Path, string Name)> { ("/app1", "WebApplication1"), ("/app2", "WebApplication2") };
        foreach (var target in targets)
        {
            var response = await httpClient.GetAsync(target.Path);
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Contains(target.Name, await response.Content.ReadAsStringAsync());
        }
    }
}