using System.Threading.Tasks;
using Shouldly;
using Uow.Mssql.Database;
using Uow.Mssql.Tests.Infrastructure;
using Xunit;

namespace Uow.Mssql.Tests;

/**
 * This is example of test where Uow isn't important and were it's more expressive to hide it behind the scenes
 */
public class EntityRepositoryTest : RepositoryTest
{
    private readonly EntityRepository _repository;

    public EntityRepositoryTest(DatabaseFixture fixture) : base(fixture)
    {
        _repository = new EntityRepository(fixture.GetConnection);
    }

    [Fact]
    public async Task Should_create_entity()
    {
        var value = 456;

        var id = await _repository.Create(value);

        var result = await _repository.GetOrDefault(id);
        result.Id.ShouldBe(id);
        result.Value.ShouldBe(value);
    }
}
