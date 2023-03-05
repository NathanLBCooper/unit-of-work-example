using System;
using Microsoft.EntityFrameworkCore;
using Uow.EntityFramework.Example.Application;
using Uow.EntityFramework.Example.Storage;

namespace Uow.EntityFramework.Tests.Infrastructure;

public class DatabaseFixture : IDisposable
{
    private readonly TestDatabaseContext _testDatabaseContext;

    public ICreateUnitOfWork CreateUnitOfWork { get; }
    public ExampleDbContext DbContext { get; }

    public DatabaseFixture()
    {
        _testDatabaseContext = new TestDatabaseContext();
        _testDatabaseContext.InitializeTestDatabase();
        var connectionString = _testDatabaseContext.ConnectionString!;

        //var sqlSettings = new SqlSettings(connectionString); // todo should UnitOfWorkContext take SqlSettings instead of DbContext?

        var options = new DbContextOptionsBuilder<ExampleDbContext>()
            .UseSqlServer(connectionString);
        DbContext = new ExampleDbContext(options.Options);

        CreateUnitOfWork = new UnitOfWorkContext(DbContext);
    }

    public void Dispose()
    {
        DbContext.Dispose();
        _testDatabaseContext.Dispose();
    }
}
