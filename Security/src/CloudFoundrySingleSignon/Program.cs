using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Steeltoe.Common.Hosting;
using Steeltoe.Connector.Redis;
using Steeltoe.Extensions.Configuration.CloudFoundry;
using Steeltoe.Management.Endpoint;
using Steeltoe.Security.Authentication.CloudFoundry;
using Steeltoe.Security.DataProtection;

var builder = WebApplication.CreateBuilder(args);
builder.UseCloudHosting(null, 8081);
builder.Configuration
    .AddCloudFoundry()
    .AddCloudFoundryContainerIdentity("a8fef16f-94c0-49e3-aa0b-ced7c3da6229", "122b942a-d7b9-4839-b26e-836654b9785f");

builder.AddAllActuators();

builder.Services.AddCloudFoundryContainerIdentity();

// AddCloudFoundryContainerIdentity adds certificate forwarding, all we need to do to use it is setup HttpClientFactory
builder.Services.AddHttpClient("default");

builder.Services.AddAuthentication(options =>
    {
        options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = CloudFoundryDefaults.AuthenticationScheme;
    })
    .AddCookie(options =>
    {
        options.AccessDeniedPath = new PathString("/Home/AccessDenied");
    })
    .AddCloudFoundryOpenIdConnect(builder.Configuration);

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("testgroup", policy => policy.RequireClaim("scope", "testgroup"));
    options.AddPolicy("testgroup1", policy => policy.RequireClaim("scope", "testgroup1"));
});

// In order to facilitate scaling beyond a single instance,
// uncomment the code below and the service binding in manifest.yml
//builder.Services.AddRedisConnectionMultiplexer(builder.Configuration);
//builder.Services.AddDataProtection()
//    .PersistKeysToRedis()
//    .SetApplicationName("cfSSO");
//builder.Services.AddDistributedRedisCache(builder.Configuration);

builder.Services.AddSession();

builder.Services.AddControllersWithViews();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
else
{
    app.UseExceptionHandler("/Home/Error");
}

app.UseStaticFiles();

app.UseForwardedHeaders(new ForwardedHeadersOptions
{
    ForwardedHeaders = ForwardedHeaders.XForwardedProto
});

app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();