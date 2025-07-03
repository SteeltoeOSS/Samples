using System;
using System.Linq;
using System.Net.Http;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Steeltoe.Common;
using Steeltoe.Common.Certificates;
using Steeltoe.Configuration.CloudFoundry;
using Steeltoe.Configuration.CloudFoundry.ServiceBindings;
using Steeltoe.Management.Endpoint.Actuators.All;
using Steeltoe.Samples.AuthWeb;
using Steeltoe.Samples.AuthWeb.ApiClients;
using Steeltoe.Security.Authentication.OpenIdConnect;
using Steeltoe.Security.Authorization.Certificate;

const string orgId = "a8fef16f-94c0-49e3-aa0b-ced7c3da6229";
const string spaceId = "122b942a-d7b9-4839-b26e-836654b9785f";

// for local testing
// Environment.SetEnvironmentVariable("VCAP_APPLICATION", "{}");

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllersWithViews();

// Steeltoe: Add Cloud Foundry application and service info to configuration.
builder.AddCloudFoundryConfiguration();
builder.Configuration.AddCloudFoundryServiceBindings();

// Steeltoe: Add instance identity certificate to configuration.
builder.Configuration.AddAppInstanceIdentityCertificate(new Guid(orgId), new Guid(spaceId));

// for local testing
// builder.Configuration.AddJsonFile("certificates.json", false, true);

// Steeltoe: Configure Microsoft's OpenIDConnect library for authentication and authorization with UAA/Cloud Foundry.
builder.Services.AddAuthentication(options =>
{
    options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = OpenIdConnectDefaults.AuthenticationScheme;
}).AddCookie(options =>
{
    options.AccessDeniedPath = new PathString("/Home/AccessDenied");
}).AddOpenIdConnect().ConfigureOpenIdConnectForCloudFoundry();

// Steeltoe: Register Microsoft authorization services and claim-based policies requiring specific scopes.
builder.Services.AddAuthorizationBuilder().AddPolicy(Globals.RequiredJwtScope, policy => policy.RequireClaim("scope", Globals.RequiredJwtScope))
    .AddPolicy(Globals.UnknownJwtScope, policy => policy.RequireClaim("scope", Globals.UnknownJwtScope));

// Steeltoe: Register HttpClients for communicating with a backend service, including an application instance certificate for authorization.
builder.Services.AddHttpClient<JwtAuthorizationApiClient>(SetBaseAddress).ConfigureLogging();
builder.Services.AddHttpClient<CertificateAuthorizationApiClient>(SetBaseAddress).AddAppInstanceIdentityCertificate().ConfigureLogging();

// Steeltoe: Add actuator endpoints.
builder.Services.AddAllActuators();

WebApplication app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapDefaultControllerRoute();

app.Run();
return;

// This code is used to limit complexity in the sample. A real application should use Service Discovery.
// To learn more about service discovery, review the documentation at: https://docs.steeltoe.io/api/v4/discovery/
static void SetBaseAddress(IServiceProvider serviceProvider, HttpClient client)
{
    string? overrideAddress = Environment.GetEnvironmentVariable("AUTHAPI_URI");

    if (string.IsNullOrEmpty(overrideAddress))
    {
        var instanceInfo = serviceProvider.GetRequiredService<IApplicationInstanceInfo>();

        if (instanceInfo is CloudFoundryApplicationOptions { Uris.Count: > 0 } options)
        {
            string address = options.Uris.First();
            string baseAddress = address.Replace("auth-client-sample", "auth-server-sample");
            client.BaseAddress = new Uri($"https://{baseAddress}");
        }
        else
        {
            client.BaseAddress = new Uri("https://localhost:7184");
        }
    }
    else
    {
        client.BaseAddress = new Uri(overrideAddress);
    }
}
