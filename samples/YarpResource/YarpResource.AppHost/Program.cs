var builder = DistributedApplication.CreateBuilder(args);

var app1 = builder.AddProject<Projects.WebApplication1>("app1");
var app2 = builder.AddProject<Projects.WebApplication2>("app2");

var isHttps = builder.Configuration["DOTNET_LAUNCH_PROFILE"] == "https";
var ingressPort = int.TryParse(builder.Configuration["Ingress:Port"], out var port) ? port : (int?)null;

builder.AddYarp("ingress")
    .WithEndpoint(scheme: isHttps ? "https" : "http", port: ingressPort)
    .WithReference(app1)
    .WithReference(app2)
    .LoadFromConfiguration("ReverseProxy");

builder.Build().Run();
