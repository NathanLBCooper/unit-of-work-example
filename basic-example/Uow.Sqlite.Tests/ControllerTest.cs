using System;
using Shouldly;
using Uow.Sqlite.Example.Application;
using Uow.Sqlite.Example.Storage;
using Uow.Sqlite.Tests.Infrastructure;
using Xunit;

namespace Uow.Sqlite.Tests;

public class ControllerTest : IDisposable
{
    /**
     * This tests "Controller", which uses unit of work
     *  This, therefore, also test the Commit(), Rollback(), etc behaviour of unit of work
     */

    private readonly DatabaseFixture _fixture = new();
    private readonly Controller _sut;

    public ControllerTest()
    {
        var repository = new EntityRepository(_fixture.GetConnection);
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

    public void Dispose()
    {
        _fixture?.Dispose();
    }
}
