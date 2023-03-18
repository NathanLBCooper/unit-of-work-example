using System;
using Shouldly;
using Uow.EntityFramework.Example.Application;
using Uow.EntityFramework.Example.Storage;
using Uow.EntityFramework.Tests.Infrastructure;
using Xunit;

namespace Uow.EntityFramework.Tests;

public class UnitOfWorkTest : IDisposable
{
    private readonly DatabaseFixture _fixture = new();
    private readonly ICreateUnitOfWork _createUnitOfWork;
    private readonly EntityRepository _repository;

    public UnitOfWorkTest()
    {
        _createUnitOfWork = _fixture.CreateUnitOfWork;
        _repository = new EntityRepository(_fixture.GetDbContext);
    }

    [Fact]
    public void Cannot_use_db_context_after_a_rollback()
    {
        int? createdId = null;
        using var uow = _createUnitOfWork.Create();
        createdId = _repository.Create(1);
        uow.RollBack();

        var attemptToContinueUsingUow = uow.Commit;
        _ = attemptToContinueUsingUow.ShouldThrow<InvalidOperationException>();

        /*
         * The rolled back changes are still in the DbContext, but that's fine, because it's unusable now
         * If DbContext lives longer than the UOW, try doing DbContext.ChangeTracker.Clear() on rollback
         */
    }

    public void Dispose()
    {
        _fixture?.Dispose();
    }
}
