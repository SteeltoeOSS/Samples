using Microsoft.AspNetCore.Authentication.Certificate;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Steeltoe.Common.Certificate;
using Steeltoe.Configuration.CloudFoundry;
using Steeltoe.Configuration.CloudFoundry.ServiceBinding;
using Steeltoe.Management.Endpoint;
using Steeltoe.Samples.AuthServer;
using Steeltoe.Security.Authentication.JwtBearer;
using Steeltoe.Security.Authorization.Certificate;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration
    .AddCloudFoundry() // needed for actuators
    .AddCloudFoundryServiceBindings()
    .AddAppInstanceIdentityCertificate(new Guid("a8fef16f-94c0-49e3-aa0b-ced7c3da6229"), new Guid("122b942a-d7b9-4839-b26e-836654b9785f"));

builder.Services
    .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer()
    .AddCertificate();

builder.Services.AddAuthorizationBuilder()
    .AddPolicy(Globals.RequiredJwtScope, policy =>
        {
            policy.RequireClaim("scope", Globals.RequiredJwtScope);
        })
    .AddPolicy(CertificateAuthorizationDefaults.SameOrganizationAuthorizationPolicy, authorizationPolicyBuilder =>
        {
            authorizationPolicyBuilder.AddAuthenticationSchemes([CertificateAuthenticationDefaults.AuthenticationScheme]);
            authorizationPolicyBuilder.RequireSameOrg();
        })
    .AddPolicy(CertificateAuthorizationDefaults.SameSpaceAuthorizationPolicy, authorizationPolicyBuilder =>
        {
            authorizationPolicyBuilder.AddAuthenticationSchemes([CertificateAuthenticationDefaults.AuthenticationScheme]);
            authorizationPolicyBuilder.RequireSameSpace();
        });

builder.Services.ConfigureJwtBearerForCloudFoundry();
builder.Services.AddCertificateAuthorizationServer();

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
