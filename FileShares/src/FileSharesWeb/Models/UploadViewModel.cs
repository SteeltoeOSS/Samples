namespace Steeltoe.Samples.FileSharesWeb.Models;

public sealed class UploadViewModel
{
    public Dictionary<string, string>? Files { get; set; }
    public string? Error { get; set; }
}
