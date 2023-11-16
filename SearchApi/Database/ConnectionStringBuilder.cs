namespace SearchApi;

public class ConnectionStringBuilder
{
    public static string Create()
    {
        string randomDatabaseName = new Random().Next(2) == 0 ? "postgres" : "postgres2";

        Console.WriteLine("Creating an instance with database: " + randomDatabaseName);

        string host = Environment.GetEnvironmentVariable("POSTGRES_HOST") ?? "localhost";
        string username = Environment.GetEnvironmentVariable("POSTGRES_USER") ?? "postgres";
        string password = Environment.GetEnvironmentVariable("POSTGRES_PASSWORD") ?? "example";

        string connectionString = $"Host={host};Username={username};Password={password};Database={randomDatabaseName}";
        return connectionString;
    }
}
