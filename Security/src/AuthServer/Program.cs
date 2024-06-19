using Microsoft.AspNetCore.Authentication.JwtBearer;
using Steeltoe.Common.Certificates;
using Steeltoe.Configuration.CloudFoundry;
using Steeltoe.Configuration.CloudFoundry.ServiceBinding;
using Steeltoe.Management.Endpoint;
using Steeltoe.Samples.AuthServer;
using Steeltoe.Security.Authentication.JwtBearer;
using Steeltoe.Security.Authorization.Certificate;

const string organizationId = "a8fef16f-94c0-49e3-aa0b-ced7c3da6229";
const string spaceId = "122b942a-d7b9-4839-b26e-836654b9785f";

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

// Steeltoe: Add Cloud Foundry application and service info and instance identity certificate to configuration
builder.Configuration
    .AddCloudFoundry()
    .AddCloudFoundryServiceBindings()
    .AddAppInstanceIdentityCertificate(new Guid(organizationId), new Guid(spaceId));

// Steeltoe: Configure Microsoft's JWT Bearer and Certificate libraries for authentication and authorization with UAA/Cloud Foundry
builder.Services
    .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer().ConfigureJwtBearerForCloudFoundry()
    .AddCertificate();

builder.Services.AddAuthorizationBuilder()
    .AddPolicy(Globals.RequiredJwtScope, policy =>
        {
            policy.RequireClaim("scope", Globals.RequiredJwtScope);
        })
    .AddOrgAndSpacePolicies();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Steeltoe: Add actuator endpoints.
builder.AddAllActuators();

var app = builder.Build();

// Steeltoe: Use certificate and header forwarding along with ASP.NET Core Authentication and Authorization middlewares
app.UseCertificateAuthorization();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapControllers();

app.Run();
