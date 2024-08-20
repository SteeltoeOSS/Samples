namespace Steeltoe.Samples.PostgreSql.Models;

public sealed class PostgreSqlViewModel
{
    public string? ConnectionString { get; set; }

    public Dictionary<string, string?> Rows { get; set; } = new();
}
