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

    public UnitOfWork(IDbContextFactory<ExampleDbContext> dbContextFactory)
    {
        DbContext = dbContextFactory.CreateDbContext();
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
