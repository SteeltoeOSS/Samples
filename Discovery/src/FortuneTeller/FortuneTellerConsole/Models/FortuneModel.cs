namespace Steeltoe.Samples.FortuneTellerConsole.Models;

public sealed class FortuneModel
{
    public long Id { get; set; }
    public string? Text { get; set; }

    public override string ToString()
    {
        return $"{Id}: '{Text}'";
    }
}
