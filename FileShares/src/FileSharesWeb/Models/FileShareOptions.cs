using System.Net;

namespace Steeltoe.Samples.FileSharesWeb.Models;

public sealed class FileShareOptions
{
    internal string Location { get; set; } = string.Empty;

    internal NetworkCredential Credential { get; set; } = new();
}
