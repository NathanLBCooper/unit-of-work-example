using System;
using Uow.Sqlite.Example.Application;
using Uow.Sqlite.Example.Storage;

namespace Uow.Sqlite.Tests.Infrastructure;

public class DatabaseFixture : IDisposable
{
    private readonly TestDatabaseContext _testDatabaseContext;

    public ICreateUnitOfWork CreateUnitOfWork;
    public IGetConnection GetConnection { get; }

    public DatabaseFixture()
    {
        _testDatabaseContext = new TestDatabaseContext();
        var connection = _testDatabaseContext.Connection;

        var unitOfWorkContext = new UnitOfWorkContext(connection);
        CreateUnitOfWork = unitOfWorkContext;
        GetConnection = unitOfWorkContext;
    }

    public void Dispose()
    {
        _testDatabaseContext.Dispose();
    }
}
