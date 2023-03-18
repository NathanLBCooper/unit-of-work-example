using Shouldly;
using Uow.EntityFramework.Example.Storage;
using Uow.EntityFramework.Tests.Infrastructure;
using Xunit;

namespace Uow.EntityFramework.Tests;

/**
 * This is example of test where Uow isn't important and were it's more expressive to hide it behind the scenes
 */
public class EntityRepositoryTest : RepositoryTest
{
    private readonly EntityRepository _repository;

    public EntityRepositoryTest(DatabaseFixture fixture) : base(fixture)
    {
        _repository = new EntityRepository(fixture.GetDbContext);
    }

    [Fact]
    public void Should_create_entity()
    {
        var value = 456;

        var id = _repository.Create(value);

        var result = _repository.GetOrDefault(id);
        _ = result.ShouldNotBeNull();
        result.Id.ShouldBe(id);
        result.Value.ShouldBe(value);
    }
}
