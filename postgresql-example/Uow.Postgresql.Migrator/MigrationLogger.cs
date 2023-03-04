using System;
using Serilog;
using SimpleMigrations;

namespace Uow.Postgresql.Migrator;

public class MigrationLogger : SimpleMigrations.ILogger
{
    /// <summary>
    /// Invoked when a sequence of migrations is started
    /// </summary>
    public void BeginSequence(MigrationData from, MigrationData to)
    {
        Log.Information($"Migrating from {from.Version}: {from.FullName} to {to.Version}: {to.FullName}");
    }

    /// <summary>
    /// Invoked when a sequence of migrations is completed successfully
    /// </summary>
    public void EndSequence(MigrationData from, MigrationData to)
    {
        Log.Information("Done");
    }

    /// <summary>
    /// Invoked when a sequence of migrations fails with an error
    /// </summary>
    public void EndSequenceWithError(Exception exception, MigrationData from, MigrationData currentVersion)
    {
        Log.Fatal("Database is currently on version {0}: {1}", currentVersion.Version, currentVersion.FullName);
    }

    // <summary>
    /// Invoked when an individual migration is started
    /// </summary>
    public void BeginMigration(MigrationData migration, MigrationDirection direction)
    {
        var term = direction == MigrationDirection.Up ? "migrating" : "reverting";
        Log.Information($"{migration.Version}: {migration.FullName} {term}");
    }

    /// <summary>
    /// Invoked when an individual migration is completed successfully
    /// </summary>
    public void EndMigration(MigrationData migration, MigrationDirection direction)
    {
    }

    /// <summary>
    /// Invoked when an individual migration fails with an error
    /// </summary>
    public void EndMigrationWithError(Exception exception, MigrationData migration, MigrationDirection direction)
    {
        Log.Fatal($"{migration.Version}: {migration.FullName} ERROR {exception.Message}");
    }

    /// <summary>
    /// Invoked when another informative message should be logged
    /// </summary>
    public void Info(string message)
    {
        Log.Information(message);
    }

    /// <summary>
    /// Invoked when SQL being executed should be logged
    /// </summary>
    public void LogSql(string message)
    {
    }
}
