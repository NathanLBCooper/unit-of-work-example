namespace Uow.Postgresql.Migrator;

public class SqlSettings
{
    public string ConnectionString { get; set; }

    public SqlSettings(string connectionString)
    {
        ConnectionString = connectionString;
    }
}
