namespace MySql.Models;

public sealed class MySqlViewModel
{
    public string? DatabaseName { get; set; }
    public string? ServerName { get; set; }

    public Dictionary<string, string?> Rows { get; set; } = new();
}
