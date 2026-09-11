using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.AspNetCore.Server.Kestrel.Https;

namespace Steeltoe.Samples.AuthApi;

internal static class LocalMutualTlsSupport
{
    private const string ForwardedClientCertHeaderName = "X-Forwarded-Client-Cert";

    // Mirrors LocalCertificateWriter's layout in Steeltoe.Common.Certificates: CA materials live one level
    // up from the app directory, under GeneratedCertificates/trust, shared across samples in the solution.
    private static readonly string TrustStorePath = ResolveTrustStorePath();
    private static readonly X509Certificate2 RootCaCertificate = LoadTrustedCertificate("SteeltoeCA.crt");
    private static readonly X509Certificate2 IntermediateCertificate = LoadTrustedCertificate("SteeltoeIntermediate.crt");

    public static IServiceCollection AddLocalMutualTlsSupport(this IServiceCollection services)
    {
        services.PostConfigure<KestrelServerOptions>(options => options.ConfigureHttpsDefaults(httpsOptions =>
        {
            httpsOptions.ClientCertificateMode = ClientCertificateMode.AllowCertificate;

            // Kestrel's default validation rejects Steeltoe certs because the Steeltoe-generated CA isn't OS-trusted.
            // Validate against the Steeltoe CA/intermediate chain directly instead of trusting the OS store.
            httpsOptions.ClientCertificateValidation = ValidateAgainstSteeltoeTrustChain;
        }));

        return services;
    }

    public static IApplicationBuilder UseLocalMutualTlsSupport(this IApplicationBuilder builder)
    {
        builder.Use(async (context, next) =>
        {
            context.Request.Headers.Remove(ForwardedClientCertHeaderName);

            if (IsLoopbackAddress(context.Connection.RemoteIpAddress) && context.Connection.ClientCertificate != null)
            {
                context.Request.Headers[ForwardedClientCertHeaderName] = Convert.ToBase64String(context.Connection.ClientCertificate.RawData);
            }

            await next(context);
        });

        return builder;
    }

    private static bool IsLoopbackAddress(IPAddress? address)
    {
        if (address == null)
        {
            return false;
        }

        if (address.IsIPv4MappedToIPv6)
        {
            address = address.MapToIPv4();
        }

        return IPAddress.IsLoopback(address);
    }

    private static bool ValidateAgainstSteeltoeTrustChain(X509Certificate2? certificate, X509Chain? remoteChain, SslPolicyErrors sslPolicyErrors)
    {
        if (certificate == null)
        {
            return false;
        }

        using var customChain = new X509Chain();
        customChain.ChainPolicy.TrustMode = X509ChainTrustMode.CustomRootTrust;
        customChain.ChainPolicy.CustomTrustStore.Add(RootCaCertificate);
        customChain.ChainPolicy.RevocationMode = X509RevocationMode.NoCheck;

        return customChain.Build(certificate);
    }

    private static string ResolveTrustStorePath()
    {
        string appBasePath = AppContext.BaseDirectory.Contains($"{Path.DirectorySeparatorChar}bin{Path.DirectorySeparatorChar}", StringComparison.Ordinal)
            ? AppContext.BaseDirectory[..AppContext.BaseDirectory.LastIndexOf($"{Path.DirectorySeparatorChar}bin", StringComparison.Ordinal)]
            : AppContext.BaseDirectory[..^1];

        string parentPath = Directory.GetParent(appBasePath)?.ToString() ?? string.Empty;
        return Path.Combine(parentPath, "GeneratedCertificates", "trust");
    }

    private static X509Certificate2 LoadTrustedCertificate(string fileName)
    {
        return X509Certificate2.CreateFromPem(File.ReadAllText(Path.Combine(TrustStorePath, fileName)));
    }
}
