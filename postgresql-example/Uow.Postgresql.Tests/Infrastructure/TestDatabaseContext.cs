using System;
using System.Reflection;
using Dapper;
using Npgsql;
using SimpleMigrations;
using SimpleMigrations.DatabaseProvider;

namespace Uow.Postgresql.Tests.Infrastructure
{
    public class TestDatabaseContext : IDisposable
    {
        private readonly string _databaseName;

        public TestDatabaseContext()
        {
            _databaseName = $"uow_postgresql_test{Guid.NewGuid()}".Replace("-", "");
        }

        public void InitializeTestDatabase()
        {
            CreateTestDatabase(_databaseName);

            ConnectionString = GetConnectionString(_databaseName);

            MigrateDatabase(ConnectionString);
        }

        public string ConnectionString { get; private set; }

        private static void CreateTestDatabase(string databaseName)
        {
            var defaultDbConnectionString = GetConnectionString("postgres");

            using var defaultDbConnection = new NpgsqlConnection(defaultDbConnectionString);
            defaultDbConnection.Execute($"create database {databaseName}");
        }

        private static void MigrateDatabase(string panelistCommunicationsConnectionString)
        {
            using var connection = new NpgsqlConnection(panelistCommunicationsConnectionString);
            var databaseProvider = new PostgresqlDatabaseProvider(connection);
            var migrator = new SimpleMigrator(typeof(Migrator.Program).GetTypeInfo().Assembly, databaseProvider);
            migrator.Load();
            migrator.MigrateToLatest();
        }

        public void Dispose()
        {
            var defaultDbConnectionString = GetConnectionString("postgres");
            using var defaultDbConnection = new NpgsqlConnection(defaultDbConnectionString);

            defaultDbConnection.Execute($"SELECT pg_terminate_backend(pg_stat_activity.pid) FROM pg_stat_activity WHERE pg_stat_activity.datname = '{_databaseName}' AND pid <> pg_backend_pid()");
            defaultDbConnection.Execute($"DROP DATABASE IF EXISTS {_databaseName}");
        }

        private static string GetConnectionString(string database) => new NpgsqlConnectionStringBuilder
        {
            Host = "localhost",  // todo for test in docker "postgres"
            Port = 15432, // todo test for docker "5432"
            Username = "postgresqluser",
            Password = "postgresqluserpassword",
            Database = database
        }.ToString();
    }
}
