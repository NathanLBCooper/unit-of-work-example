using System;
using System.Data;
using Uow.Mssql.Database;
using Uow.Mssql.Database.UnitOfWork;

namespace Uow.Mssql.Tests.Infrastructure;

public class DatabaseFixture : IDisposable
{
    private readonly ICreateUnitOfWork _createUnitOfWork;
    private readonly TestDatabaseContext _testDatabaseContext;

    public IGetConnection GetConnection { get; }

    public DatabaseFixture()
    {
        _testDatabaseContext = new TestDatabaseContext();
        _testDatabaseContext.InitializeTestDatabase();
        var connectionString = _testDatabaseContext.ConnectionString!;

        var sqlSettings = new SqlSettings(connectionString);

        var unitOfWorkContext = new UnitOfWorkContext(sqlSettings);
        _createUnitOfWork = unitOfWorkContext;
        GetConnection = unitOfWorkContext;
    }

    public (IDbConnection connection, IDbTransaction transaction) GetCurrentConnection()
    {
        return GetConnection.GetConnection();
    }

    public IUnitOfWork CreateUnitOfWork()
    {
        return _createUnitOfWork.Create();
    }

    public void Dispose()
    {
        _testDatabaseContext.Dispose();
    }
}
