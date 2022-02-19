using System;
using System.Data.SQLite;
using Dapper;

namespace Uow.Sqlite.Tests.Infrastructure
{
    public class TestDatabaseContext : IDisposable
    {
        public TestDatabaseContext()
        {
            Connection = InitializeTestDatabase();
        }

        public SQLiteConnection Connection { get; private set; }

        public void Dispose()
        {
            Connection?.Dispose();
        }

        private static SQLiteConnection InitializeTestDatabase()
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
