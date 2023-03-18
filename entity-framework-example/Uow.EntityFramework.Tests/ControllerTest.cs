using Shouldly;
using Uow.EntityFramework.Example.Application;
using Uow.EntityFramework.Example.Storage;
using Uow.EntityFramework.Tests.Infrastructure;
using Xunit;

namespace Uow.EntityFramework.Tests;

public class ControllerTest : IClassFixture<DatabaseFixture>
{
    /**
     * This tests "Controller", which uses unit of work
     *  This, therefore, also test the Commit(), Rollback(), etc behaviour of unit of work
     */

    private readonly DatabaseFixture _fixture;
    private readonly Controller _sut;

    public ControllerTest(DatabaseFixture fixture)
    {
        _fixture = fixture;
        var repository = new EntityRepository(_fixture.GetDbContext);
        _sut = new Controller(repository, _fixture.CreateUnitOfWork);
    }

    [Fact]
    public void Creating_an_entity_and_commiting_saves_to_the_db()
    {
        var expectedEntity = new Entity { Id = null, Value = 10 };

        var createdId = _sut.CreateAndCommit(expectedEntity);
        var createdEntity = _sut.GetOrDefault(createdId);

        _ = createdEntity.ShouldNotBeNull();
        createdEntity.Value.ShouldBe(expectedEntity.Value);
    }

    [Fact]
    public void Creating_an_entity_and_rolling_back_saves_nothing()
    {
        var expectedEntity = new Entity { Id = null, Value = 10 };

        var createdId = _sut.CreateAndRollback(expectedEntity);
        var createdEntity = _sut.GetOrDefault(createdId);

        createdEntity.ShouldBeNull();
    }

    [Fact]
    public void Creating_an_entity_and_not_commiting_saves_nothing()
    {
        var expectedEntity = new Entity { Id = null, Value = 10 };

        var createdId = _sut.CreateAndNeitherCommitNorRollback(expectedEntity);
        var createdEntity = _sut.GetOrDefault(createdId);

        createdEntity.ShouldBeNull();
    }
}
