using System;
using Microsoft.EntityFrameworkCore.Storage;
using Uow.EntityFramework.Example.Application;

namespace Uow.EntityFramework.Example.Storage;

public class UnitOfWork : IUnitOfWork, IDisposable
{
    public ExampleDbContext DbContext { get; }
    private readonly IDbContextTransaction _transaction;

    public bool IsDisposed { get; private set; } = false;

    public UnitOfWork(ExampleDbContext dbContext)
    {
        DbContext = dbContext;
        _transaction = DbContext.Database.BeginTransaction(); // todo is this right? Maybe the DbContext lifetime should be the same as the UOW?
    }

    public void RollBack()
    {
        _transaction.Rollback();
        DbContext.ChangeTracker.Clear();
    }

    public void Commit()
    {
        _transaction.Commit();
    }

    public void Dispose()
    {
        _transaction?.Dispose();
        DbContext.ChangeTracker.Clear();

        IsDisposed = true;
    }
}
