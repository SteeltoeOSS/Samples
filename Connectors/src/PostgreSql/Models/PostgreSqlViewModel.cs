namespace PostgreSql.Models;

public sealed class PostgreSqlViewModel
{
    public string? DatabaseName { get; set; }
    public string? ServerName { get; set; }

    public Dictionary<string, string?> Rows { get; set; } = new();
}
