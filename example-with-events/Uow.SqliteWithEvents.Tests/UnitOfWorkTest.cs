using System;
using Shouldly;
using Uow.SqliteWithEvents.Database;
using Uow.SqliteWithEvents.Example;
using Uow.SqliteWithEvents.Example.UnitOfWork;
using Uow.SqliteWithEvents.Tests.Infrastructure;
using Xunit;

namespace Uow.SqliteWithEvents.Tests;

public class UnitOfWorkTest : IDisposable
{
    private readonly DatabaseFixture _fixture = new();
    private readonly EntityRepository _repository;
    private readonly ICreateUnitOfWork _createUnitOfWork;

    public UnitOfWorkTest()
    {
        _repository = new EntityRepository(_fixture.GetConnection);
        _createUnitOfWork = _fixture.CreateUnitOfWork;
    }

    [Fact]
    public void Creating_an_entity_and_commiting_saves_to_the_db_and_publishes_an_event()
    {
        var expectedEntity = new Entity { Id = null, Value = 10 };

        using (var uow = _createUnitOfWork.Create())
        {
            expectedEntity.Id = _repository.Create(expectedEntity.Value);

            uow.Commit();
        }

        var @event = _fixture.CurrentEventPublisher!.Committed.ShouldHaveSingleItem();
        @event.Id.ShouldBe(expectedEntity.Id.Value);

        using (_createUnitOfWork.Create())
        {
            var entity = _repository.GetOrDefault(expectedEntity.Id.Value);
            _ = entity.ShouldNotBeNull();
            entity.Value.ShouldBe(expectedEntity.Value);
        }
    }

    [Fact]
    public void Creating_an_entity_and_rolling_back_saves_nothing_and_publishes_no_events()
    {
        var expectedEntity = new Entity { Id = null, Value = 10 };

        using (var uow = _createUnitOfWork.Create())
        {
            expectedEntity.Id = _repository.Create(expectedEntity.Value);

            uow.RollBack();
        }

        _fixture.CurrentEventPublisher!.Committed.ShouldBeEmpty();

        using (_createUnitOfWork.Create())
        {
            var entity = _repository.GetOrDefault(expectedEntity.Id.Value);
            entity.ShouldBeNull();
        }
    }

    [Fact]
    public void Creating_an_entity_and_not_commiting_saves_nothing_and_publishes_no_events()
    {
        var expectedEntity = new Entity { Id = null, Value = 10 };

        using (var uow = _createUnitOfWork.Create())
        {
            expectedEntity.Id = _repository.Create(expectedEntity.Value);
        }

        _fixture.CurrentEventPublisher!.Committed.ShouldBeEmpty();

        using (_createUnitOfWork.Create())
        {
            var entity = _repository.GetOrDefault(expectedEntity.Id.Value);
            entity.ShouldBeNull();
        }
    }

    public void Dispose()
    {
        _fixture?.Dispose();
    }
}
