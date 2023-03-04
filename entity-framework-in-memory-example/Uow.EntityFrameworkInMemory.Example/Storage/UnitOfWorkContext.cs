using System;
using Uow.EntityFrameworkInMemory.Example.Application;

namespace Uow.EntityFrameworkInMemory.Example.Storage;

public class UnitOfWorkContext : ICreateUnitOfWork
{
    private readonly ExampleDbContext _dbContext;
    private UnitOfWork? _unitOfWork;

    private bool IsUnitOfWorkOpen => !(_unitOfWork == null || _unitOfWork.IsDisposed);

    public UnitOfWorkContext(ExampleDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public ExampleDbContext GetConnection()
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

        _unitOfWork = new UnitOfWork(_dbContext);
        return _unitOfWork;
    }
}
