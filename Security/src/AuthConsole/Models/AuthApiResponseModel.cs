namespace Steeltoe.Samples.AuthConsole.Models;

public sealed class AuthApiResponseModel
{
    public string? Message { get; set; }
    public Exception? Error { get; set; }
}
