using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Steeltoe.Common.Hosting;
using Steeltoe.Extensions.Configuration.CloudFoundry;
using Steeltoe.Management.Endpoint;
using Steeltoe.Security.Authentication.CloudFoundry;

var builder = WebApplication.CreateBuilder(args);
builder.Configuration.AddCloudFoundry();
builder.UseCloudHosting(null, 8083);
builder.AddAllActuators();

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
   .AddCloudFoundryJwtBearer(builder.Configuration);
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("testgroup", policy => policy.RequireClaim("scope", "testgroup"));
    options.AddPolicy("testgroup1", policy => policy.RequireClaim("scope", "testgroup1"));
});

builder.Services.AddControllersWithViews();

var app = builder.Build();

app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();