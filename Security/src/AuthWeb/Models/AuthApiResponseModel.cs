namespace Steeltoe.Samples.AuthWeb.Models;

public sealed class AuthApiResponseModel
{
    public string? RequestUri { get; set; }
    public string? Message { get; set; }
    public Exception? Error { get; set; }
}
