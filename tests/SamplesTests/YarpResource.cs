using System.Net;
using Microsoft.Extensions.DependencyInjection;

namespace SamplesTests.Tests;
public class YarpResource
{
    [Fact]
    public async Task GetWebResourceRootReturnsOkStatusCode()
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
        var httpClient = app.CreateHttpClient("ingress");

        var app1Response = await httpClient.GetAsync("/app1");
        Assert.Equal(HttpStatusCode.OK, app1Response.StatusCode);

        var app2Response = await httpClient.GetAsync("/app2");
        Assert.Equal(HttpStatusCode.OK, app2Response.StatusCode);
    }
}