using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Steeltoe.Common.Certificate;
using Steeltoe.Configuration.CloudFoundry;
using Steeltoe.Configuration.CloudFoundry.ServiceBinding;
using Steeltoe.Management.Endpoint;
using Steeltoe.Security.Authentication.OpenIdConnect;
using Steeltoe.Security.Authorization.Certificate;
using System;
using Steeltoe.Samples.AuthClient;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration
    .AddCloudFoundry() // needed for actuators
    .AddCloudFoundryServiceBindings()
    .AddAppInstanceIdentityCertificate(new Guid("a8fef16f-94c0-49e3-aa0b-ced7c3da6229"), new Guid("122b942a-d7b9-4839-b26e-836654b9785f"));

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
    .AddOpenIdConnect();
builder.Services.ConfigureOpenIdConnectForCloudFoundry();

builder.Services.AddSession();
builder.Services.AddAuthorizationBuilder()
    .AddPolicy(Globals.RequiredJwtScope, policy => policy.RequireClaim("scope", Globals.RequiredJwtScope))
    .AddPolicy(Globals.UnknownJwtScope, policy => policy.RequireClaim("scope", Globals.UnknownJwtScope));

builder.Services.AddCertificateAuthorizationClient();

builder.Services.AddControllersWithViews();

builder.AddAllActuators();

var app = builder.Build();

app.UseForwardedHeaders(new ForwardedHeadersOptions { ForwardedHeaders = ForwardedHeaders.XForwardedHost | ForwardedHeaders.XForwardedProto });

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
