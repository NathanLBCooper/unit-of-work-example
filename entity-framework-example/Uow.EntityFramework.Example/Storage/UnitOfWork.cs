using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Uow.EntityFramework.Example.Application;

namespace Uow.EntityFramework.Example.Storage;

public class UnitOfWork : IUnitOfWork, IDisposable
{
    private readonly IDbContextTransaction _transaction;

    public ExampleDbContext DbContext { get; }

    public bool IsDisposed { get; private set; } = false;

    public UnitOfWork(SqlSettings sqlSettings)
    {
        var options = new DbContextOptionsBuilder<ExampleDbContext>()
            .UseSqlServer(sqlSettings.ConnectionString);
        DbContext = new ExampleDbContext(options.Options);

        _transaction = DbContext.Database.BeginTransaction();
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
        DbContext?.Dispose();

        IsDisposed = true;
    }
}
