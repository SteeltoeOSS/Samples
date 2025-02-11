namespace Steeltoe.Samples.MySql.Models;

public sealed class MySqlViewModel
{
    public string? ConnectionString { get; set; }

    public Dictionary<string, string?> Rows { get; set; } = new();
}
