using Shouldly;
using Uow.Sqlite.Example.Storage;
using Uow.Sqlite.Tests.Infrastructure;
using Xunit;

namespace Uow.Sqlite.Tests;

public class EntityRepositoryTest : RepositoryTest
{
    /**
     * This tests the repository, which is inside the unit of work.
     *  It's entirely possible to wrap every test inside a transaction, but "RepositoryTest" does that by magic,
     *      allowing one to write repository test on the real DB without worrying about unit of work
     *  Additionally, "RepositoryTest" rolls back any changes, providing additional test isolation, which may be useful when using a non-in-memory database
     */

    private readonly EntityRepository _repository;

    public EntityRepositoryTest()
    {
        _repository = new EntityRepository(Fixture.GetConnection);
    }

    [Fact]
    public void Create_should_create_entity()
    {
        var value = 456;

        var id = _repository.Create(value);

        var result = _repository.GetOrDefault(id);
        _ = result.ShouldNotBeNull();
        result.Id.ShouldBe(id);
        result.Value.ShouldBe(value);
    }
}
