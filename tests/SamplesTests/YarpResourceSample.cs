using System.Net;
using Microsoft.Extensions.DependencyInjection;

namespace SamplesTests.Tests;

public class YarpResourceSample
{
    [Fact]
    public async Task GetAppsThroughYarpIngressResourceReturnsOkStatusCode()
    {
        // Arrange
        var appHost = await DistributedApplicationTestingBuilder.CreateAsync<Projects.YarpResource_AppHost>();
        appHost.Services.ConfigureHttpClientDefaults(options =>
        {
            options.AddStandardResilienceHandler();
        });
        await using var app = await appHost.BuildAsync();
        await app.StartAsync();

        // Act
        var httpClient = app.CreateHttpClient("ingress", "http");

        var app1Response = await httpClient.GetAsync("/app1");
        Assert.Equal(HttpStatusCode.OK, app1Response.StatusCode);

        var app2Response = await httpClient.GetAsync("/app2");
        Assert.Equal(HttpStatusCode.OK, app2Response.StatusCode);
    }
}