namespace Shurl.Settings;

public class PostgresSettings
{
    public string Host { get; set; } = "";
    public string Port { get; set; } = "";
    public string DbName { get; set; } = "";
    public string User { get; set; } = "";
    public string Password { get; set; } = "";

    public string ConnectionString
    {
        get => $"Host={Host};Port={Port};Database={DbName};Username={User};Password={Password}";
    }
}
