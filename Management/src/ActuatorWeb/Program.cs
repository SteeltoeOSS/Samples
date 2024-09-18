// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information.

using ActuatorWeb.Pages;
using Steeltoe.Common;
using Steeltoe.Configuration.CloudFoundry;
using Steeltoe.Management.Endpoint;
using Steeltoe.Management.Endpoint.SpringBootAdminClient;
using Steeltoe.Management.Prometheus;
using Steeltoe.Samples.ActuatorWeb;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();

if (builder.Configuration.GetValue<bool>("UseSpringBootAdmin"))
{
    builder.Services.AddSpringBootAdminClient();
    builder.Services.SecureActuatorsWithBasicAuth();
}
else
{
    builder.AddCloudFoundryConfiguration();
}

// Steeltoe: Add actuator endpoints, with an authorization policy outside of Cloud Foundry
builder.AddAllActuators(configureEndpoints =>
{
    if (builder.Configuration.GetValue<bool>("UseSpringBootAdmin"))
    {
        configureEndpoints.RequireAuthorization("actuators.read");
    }
});
builder.Services.AddPrometheusActuator();

builder.Services.ConfigureOpenTelemetry(builder.Configuration);

// Steeltoe: Register HttpClients for communicating with a backend service, including an application instance certificate for authorization.
builder.Services.AddHttpClient<WeatherModel>(SetBaseAddress);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

// Causes certificate trust issues with SBA, port issues with ManagementPortMiddleware
//app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapRazorPages();

app.MapPrometheusActuator();

app.Run();
return;

static void SetBaseAddress(IServiceProvider serviceProvider, HttpClient client)
{
    var instanceInfo = serviceProvider.GetRequiredService<IApplicationInstanceInfo>();
    if (instanceInfo is CloudFoundryApplicationOptions { Uris.Count: > 0 } options)
    {
        string address = options.Uris.First();
        string baseAddress = address.Replace("actuator-web", "actuator-api");
        client.BaseAddress = new Uri($"https://{baseAddress}");
    }
    else
    {
        client.BaseAddress = new Uri("http://localhost:5140");
    }

    var loggerFactory = serviceProvider.GetRequiredService<ILoggerFactory>();
    var logger = loggerFactory.CreateLogger("HttpClientSetup");

    logger.LogInformation("HttpClient BaseAddress set to {BaseAddress}", client.BaseAddress);
}
