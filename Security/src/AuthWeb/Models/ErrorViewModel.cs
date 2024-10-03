namespace Steeltoe.Samples.AuthWeb.Models;

public sealed class ErrorViewModel
{
    public string? RequestId { get; init; }

    public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);
}
