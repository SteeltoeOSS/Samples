using Microsoft.AspNetCore.Authentication.Certificate;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Steeltoe.Common.Security;
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
    .AddContainerIdentityCertificate(new Guid("a8fef16f-94c0-49e3-aa0b-ced7c3da6229"), new Guid("122b942a-d7b9-4839-b26e-836654b9785f"))
;

builder.Services.ConfigureJwtBearerForCloudFoundry();
builder.Services.AddCertificateAuthorizationServer(builder.Configuration);

builder.Services
    .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer()
    .AddCertificate();

builder.Services.AddAuthorization(options =>
    {
        options.AddPolicy(Globals.RequiredJwtScope, policy =>
        {
            policy.RequireClaim("scope", Globals.RequiredJwtScope);
        });

        options.AddPolicy(CertificateAuthorizationDefaults.SameOrganizationAuthorizationPolicy, authorizationPolicyBuilder =>
        {
            authorizationPolicyBuilder.AddAuthenticationSchemes([CertificateAuthenticationDefaults.AuthenticationScheme]);
            authorizationPolicyBuilder.SameOrg();
        });

        options.AddPolicy(CertificateAuthorizationDefaults.SameSpaceAuthorizationPolicy, authorizationPolicyBuilder =>
        {
            authorizationPolicyBuilder.AddAuthenticationSchemes([CertificateAuthenticationDefaults.AuthenticationScheme]);
            authorizationPolicyBuilder.SameSpace();
        });
    });

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
