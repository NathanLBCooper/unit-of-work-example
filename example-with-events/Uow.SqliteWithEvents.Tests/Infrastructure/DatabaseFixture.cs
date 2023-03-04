using System;
using Uow.SqliteWithEvents.Example.UnitOfWork;
using Uow.SqliteWithEvents.Tests.Fakes;

namespace Uow.SqliteWithEvents.Tests.Infrastructure;

public class DatabaseFixture : IDisposable
{
    private readonly TestDatabaseContext _testDatabaseContext;
    private readonly FakeTransactionalEventPublisherFactory _fakeTransactionalEventPublisherFactory;

    public ICreateUnitOfWork CreateUnitOfWork;
    public IGetConnection GetConnection { get; }
    public FakeEventPublisher? CurrentEventPublisher => _fakeTransactionalEventPublisherFactory.CurrentEventPublisher;

    public DatabaseFixture()
    {
        _testDatabaseContext = new TestDatabaseContext();
        var connection = _testDatabaseContext.Connection;

        _fakeTransactionalEventPublisherFactory = new FakeTransactionalEventPublisherFactory();
        var unitOfWorkContext = new UnitOfWorkContext(connection, _fakeTransactionalEventPublisherFactory);
        CreateUnitOfWork = unitOfWorkContext;
        GetConnection = unitOfWorkContext;
    }

    public void Dispose()
    {
        _testDatabaseContext.Dispose();
    }
}
