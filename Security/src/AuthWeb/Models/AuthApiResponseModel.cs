using System;

namespace Steeltoe.Samples.AuthWeb.Models;

public sealed class AuthApiResponseModel
{
    public string? Message { get; set; }
    public Exception? Error { get; set; }
}
