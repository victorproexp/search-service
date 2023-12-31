namespace SearchApi;

public class ConnectionStringBuilder
{
    public static string Create(string databaseName)
    {
        string host = Environment.GetEnvironmentVariable("POSTGRES_HOST") ?? "localhost";
        string username = Environment.GetEnvironmentVariable("POSTGRES_USER") ?? "postgres";
        string password = Environment.GetEnvironmentVariable("POSTGRES_PASSWORD") ?? "example";

        string connectionString = $"Host={host};Username={username};Password={password};Database={databaseName}";
        return connectionString;
    }
}
