using Microsoft.EntityFrameworkCore;
using Shouldly;
using Uow.EntityFrameworkInMemory.Example.Application;
using Uow.EntityFrameworkInMemory.Example.Storage;
using Xunit;

namespace Uow.EntityFrameworkInMemory.Tests;

public class UnitTest1
{
    [Fact]
    public void Test1()
    {
        var options = new DbContextOptionsBuilder<ExampleDbContext>().UseInMemoryDatabase(databaseName: "todothinkofname").Options;

        using var dbContext = new ExampleDbContext(options);

        var repository = new EntityRepository(dbContext);
        var uowContext = new UnitOfWorkContext(dbContext);

        var sut = new Controller(repository, uowContext);

        var id = sut.CreateAndCommit(new Entity { Value = 10 });

        var fetchedId = sut.GetOrDefault(id);
        _ = fetchedId.ShouldNotBeNull();
    }
}
