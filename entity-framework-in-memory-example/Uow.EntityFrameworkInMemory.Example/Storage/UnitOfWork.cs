using System;
using Uow.EntityFrameworkInMemory.Example.Application;

namespace Uow.EntityFrameworkInMemory.Example.Storage;

public class UnitOfWork : IUnitOfWork, IDisposable
{
    public ExampleDbContext DbContext { get; }
    // private readonly IDbContextTransaction _transaction;

    public bool IsDisposed { get; private set; } = false;

    public UnitOfWork(ExampleDbContext dbContext)
    {
        DbContext = dbContext;
        //_transaction = DbContext.Database.BeginTransaction();
    }

    public void RollBack()
    {
        //_transaction.Rollback();
    }

    public void Commit()
    {
        //_transaction.Commit();
    }

    public void Dispose()
    {
        //_transaction?.Dispose();

        IsDisposed = true;
    }
}

public class InMemoryUnitOfWork : IUnitOfWork, IDisposable
{
    public ExampleDbContext DbContext { get; }
    public bool IsDisposed { get; private set; } = false;

    public InMemoryUnitOfWork(ExampleDbContext dbContext)
    {
        DbContext = dbContext;
    }

    public void RollBack()
    {
        // todo panic
    }

    public void Commit()
    {
    }

    public void Dispose()
    {
        // todo panic if not committed
        IsDisposed = true;
    }
}

// todo transactions not supported in memory 
