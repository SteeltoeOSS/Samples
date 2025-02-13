﻿namespace Steeltoe.Samples.FortuneTellerWeb.Models;

public sealed class FortuneModel
{
    public long Id { get; set; }
    public string? Text { get; set; }

    public override string ToString()
    {
        return $"{Id}: '{Text}'";
    }
}
