namespace Steeltoe.Samples.PostgreSql.Models;

public sealed class PostgreSqlViewModel
{
    public string? ConnectionString { get; set; }

    public IDictionary<string, string?> Rows { get; } = new Dictionary<string, string?>();
}
