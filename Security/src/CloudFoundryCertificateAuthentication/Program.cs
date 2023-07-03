using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Steeltoe.Common.Hosting;
using Steeltoe.Extensions.Configuration.CloudFoundry;
using Steeltoe.Management.Endpoint;
using Steeltoe.Security.Authentication.CloudFoundry;

var builder = WebApplication.CreateBuilder(args);
builder.UseCloudHosting(null, 8085);
builder.Configuration
    .AddCloudFoundry()
    .AddCloudFoundryContainerIdentity("a8fef16f-94c0-49e3-aa0b-ced7c3da6229", "122b942a-d7b9-4839-b26e-836654b9785f");

builder.AddAllActuators();

builder.Services.AddCloudFoundryCertificateAuth();
builder.Services.AddControllers();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
else
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseCloudFoundryCertificateAuth();

app.MapControllers();

app.Run();
