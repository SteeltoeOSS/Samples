using System;
using System.Linq;
using System.Net.Http;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Steeltoe.Common;
using Steeltoe.Common.Certificates;
using Steeltoe.Configuration.CloudFoundry;
using Steeltoe.Configuration.CloudFoundry.ServiceBinding;
using Steeltoe.Management.Endpoint;
using Steeltoe.Samples.AuthClient;
using Steeltoe.Security.Authentication.OpenIdConnect;
using Steeltoe.Security.Authorization.Certificate;

const string organizationId = "a8fef16f-94c0-49e3-aa0b-ced7c3da6229";
const string spaceId = "122b942a-d7b9-4839-b26e-836654b9785f";

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

// Steeltoe: Add Cloud Foundry application and service info and instance identity certificate to configuration
builder.Configuration
    .AddCloudFoundry()
    .AddCloudFoundryServiceBindings()
    .AddAppInstanceIdentityCertificate(new Guid(organizationId), new Guid(spaceId));

// Steeltoe: Configure Microsoft's OpenIDConnect library for authentication and authorization with UAA/Cloud Foundry
builder.Services
    .AddAuthentication(options =>
    {
        options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = OpenIdConnectDefaults.AuthenticationScheme;
    })
    .AddCookie(options =>
    {
        options.AccessDeniedPath = new PathString("/Home/AccessDenied");
    })
    .AddOpenIdConnect()
    .ConfigureOpenIdConnectForCloudFoundry();

// Steeltoe: register Microsoft authorization services and claim-based policies requiring specific scopes
builder.Services.AddAuthorizationBuilder()
    .AddPolicy(Globals.RequiredJwtScope, policy => policy.RequireClaim("scope", Globals.RequiredJwtScope))
    .AddPolicy(Globals.UnknownJwtScope, policy => policy.RequireClaim("scope", Globals.UnknownJwtScope));

// Steeltoe: Register application instance information
if (Platform.IsCloudFoundry)
{
    builder.Services.RegisterCloudFoundryApplicationInstanceInfo();
}
else
{
    builder.Services.RegisterDefaultApplicationInstanceInfo();
}

// Steeltoe: Register HttpClients for communicating with a backend service, including an application instance certificate for authorization
builder.Services.AddHttpClient("default", SetBaseAddress);
builder.Services.AddHttpClient("AppInstanceIdentity", SetBaseAddress).AddAppInstanceIdentityCertificate();

builder.Services.AddControllersWithViews();

// Steeltoe: Add actuator endpoints.
builder.AddAllActuators();

WebApplication app = builder.Build();

// Steeltoe: Direct ASP.NET Core to use forwarded header information in order to generate links correctly when behind a reverse-proxy (eg: when in Cloud Foundry)
app.UseForwardedHeaders(new ForwardedHeadersOptions
{
    ForwardedHeaders = ForwardedHeaders.XForwardedHost | ForwardedHeaders.XForwardedProto
});

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

app.UseAuthorization();

app.MapDefaultControllerRoute();

app.Run();
return;

void SetBaseAddress(IServiceProvider serviceProvider, HttpClient client)
{
    var instanceInfo = serviceProvider.GetRequiredService<IApplicationInstanceInfo>();

    if (instanceInfo.Uris != null && instanceInfo.Uris.Any())
    {
        string? address = instanceInfo.Uris.First();

        if (address == null)
        {
            throw new NotImplementedException();
        }

        string baseAddress = address.Replace("steeltoe-samples-authclient", "steeltoe-samples-authserver");
        client.BaseAddress = new Uri($"https://{baseAddress}");
    }
    else
    {
        client.BaseAddress = new Uri("https://localhost:7184");
    }
}
