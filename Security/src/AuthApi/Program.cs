using System.Security.Cryptography.X509Certificates;
using Microsoft.AspNetCore.Authentication;
using Steeltoe.Common.Certificates;
using Steeltoe.Configuration.CloudFoundry;
using Steeltoe.Configuration.CloudFoundry.ServiceBindings;
using Steeltoe.Management.Endpoint.Actuators.All;
using Steeltoe.Samples.AuthApi;
using Steeltoe.Security.Authentication.JwtBearer;
using Steeltoe.Security.Authorization.Certificate;

const string orgId = "a8fef16f-94c0-49e3-aa0b-ced7c3da6229";
const string spaceId = "122b942a-d7b9-4839-b26e-836654b9785f";

// for local testing
//Environment.SetEnvironmentVariable("VCAP_APPLICATION", "{}");
//Environment.SetEnvironmentVariable("CF_SYSTEM_CERT_PATH", "cf-system-certificates");

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Steeltoe: Add Cloud Foundry application and service info to configuration.
builder.AddCloudFoundryConfiguration();
builder.Configuration.AddCloudFoundryServiceBindings();

// Steeltoe: Add instance identity certificate to configuration.
builder.Configuration.AddAppInstanceIdentityCertificate(new Guid(orgId), new Guid(spaceId));

// for local testing
//builder.Configuration.AddJsonFile("certificates.json", false, true);

// Steeltoe: Register Microsoft's JWT Bearer and Certificate libraries for authentication, configure JWT to work with UAA/Cloud Foundry.
AuthenticationBuilder authenticationBuilder = builder.Services.AddAuthentication()/*.AddJwtBearer().ConfigureJwtBearerForCloudFoundry()*/;

authenticationBuilder.AddCertificate(
    certificateAuthenticationOptions =>
    {
        string[] certificateFilePaths = Directory.GetFiles("additional-certificates", "*.crt");
        X509Certificate2[] certificates = [.. certificateFilePaths.Select(certificatePath => new X509Certificate2(certificatePath))];
        certificateAuthenticationOptions.AdditionalChainCertificates.AddRange(certificates);
    });

// Steeltoe: Register Microsoft authorization services.
builder.Services.AddAuthorizationBuilder()
    // Steeltoe: Register a claim-based policy requiring a specific scope.
    .AddPolicy(Globals.RequiredJwtScope, policy =>
    {
        policy.RequireClaim("scope", Globals.RequiredJwtScope);
    })
    // Steeltoe: Register policies requiring space or org to match between client and server certificates.
    .AddOrgAndSpacePolicies();

// Steeltoe: Add actuator endpoints.
builder.Services.AddAllActuators();

WebApplication app = builder.Build();

// Steeltoe: Use certificate and header forwarding along with ASP.NET Core Authentication and Authorization middleware.
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
