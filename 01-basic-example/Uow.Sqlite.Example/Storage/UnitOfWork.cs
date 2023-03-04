using System;
using System.Data;
using System.Data.SQLite;
using Uow.Sqlite.Example.Application;

namespace Uow.Sqlite.Example.Storage;

public class UnitOfWork : IUnitOfWork, IDisposable
{
    private readonly SQLiteConnection _connection;
    private readonly SQLiteTransaction _transaction;

    public IDbConnection Connection => _connection;

    public bool IsDisposed { get; private set; } = false;

    public UnitOfWork(SQLiteConnection openConnection)
    {
        _connection = openConnection;
        _transaction = _connection.BeginTransaction();
    }

    public void RollBack()
    {
        _transaction.Rollback();
    }

    public void Commit()
    {
        _transaction.Commit();
    }

    public void Dispose()
    {
        _transaction?.Dispose();

        IsDisposed = true;
    }
}
