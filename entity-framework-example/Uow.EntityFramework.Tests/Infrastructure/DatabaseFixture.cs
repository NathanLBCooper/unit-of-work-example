using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Uow.EntityFramework.Example.Application;
using Uow.EntityFramework.Example.Storage;

namespace Uow.EntityFramework.Tests.Infrastructure;

public class DatabaseFixture : IDisposable
{
    private readonly TestDatabaseContext _testDatabaseContext;

    public ICreateUnitOfWork CreateUnitOfWork { get; }
    public IGetDbContext GetDbContext { get; }

    public DatabaseFixture()
    {
        _testDatabaseContext = new TestDatabaseContext();
        _testDatabaseContext.InitializeTestDatabase();
        var connectionString = _testDatabaseContext.ConnectionString!;

        var options = new DbContextOptionsBuilder<ExampleDbContext>()
            .UseSqlServer(connectionString);
        var dbContextFactory = new PooledDbContextFactory<ExampleDbContext>(options.Options);

        var unitOfWorkContext = new UnitOfWorkContext(dbContextFactory);

        CreateUnitOfWork = unitOfWorkContext;
        GetDbContext = unitOfWorkContext;
    }

    public void Dispose()
    {
        _testDatabaseContext.Dispose();
    }
}
