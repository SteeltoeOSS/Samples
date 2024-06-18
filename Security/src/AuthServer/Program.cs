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

builder.Configuration
    .AddCloudFoundry() // needed for actuators
    .AddCloudFoundryServiceBindings()
    .AddAppInstanceIdentityCertificate(new Guid(organizationId), new Guid(spaceId));

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

builder.AddAllActuators();

var app = builder.Build();

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
