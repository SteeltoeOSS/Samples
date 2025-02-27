namespace Steeltoe.Samples.MySql.Models;

public sealed class MySqlViewModel
{
    public string? ConnectionString { get; set; }

    public IDictionary<string, string?> Rows { get; } = new Dictionary<string, string?>();
}
