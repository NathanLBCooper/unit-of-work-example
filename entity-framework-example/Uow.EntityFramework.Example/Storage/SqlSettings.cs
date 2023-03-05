namespace Uow.EntityFramework.Example.Storage;

public class SqlSettings
{
    public string ConnectionString { get; set; }

    public SqlSettings(string connectionString)
    {
        ConnectionString = connectionString;
    }
}
