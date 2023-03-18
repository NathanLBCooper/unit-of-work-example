using System;
using Microsoft.EntityFrameworkCore;
using Uow.EntityFramework.Example.Application;

namespace Uow.EntityFramework.Example.Storage;

public class UnitOfWorkContext : ICreateUnitOfWork, IGetDbContext
{
    private readonly IDbContextFactory<ExampleDbContext> _dbContextFactory;
    private UnitOfWork? _unitOfWork;

    private bool IsUnitOfWorkOpen => !(_unitOfWork == null || _unitOfWork.IsDisposed);

    public UnitOfWorkContext(IDbContextFactory<ExampleDbContext> dbContextFactory)
    {
        _dbContextFactory = dbContextFactory;
    }

    public ExampleDbContext GetDbContext()
    {
        if (!IsUnitOfWorkOpen)
        {
            throw new InvalidOperationException(
                "There is not current unit of work from which to get a connection. Call Create first");
        }

        return _unitOfWork!.DbContext;
    }

    public IUnitOfWork Create()
    {
        if (IsUnitOfWorkOpen)
        {
            throw new InvalidOperationException(
                "Cannot begin a transaction before the unit of work from the last one is disposed");
        }

        _unitOfWork = new UnitOfWork(_dbContextFactory);
        return _unitOfWork;
    }
}
