using System.Data.SQLite;
using Dapper;

namespace Uow.Sqlite.Tests
{
    public static class TestDatabase
    {
        public static SQLiteConnection CreateConnectionAndSetupDatabase()
        {
            var connection = new SQLiteConnection("Data Source=:memory:");
            connection.Open();
            connection.Execute(
                @"
create table Entity (
    Id integer primary key,
    Value integer not null
);");
            return connection;
        }
    }
}
