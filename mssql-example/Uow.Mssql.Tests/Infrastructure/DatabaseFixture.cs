using System;
using System.Data;
using Uow.Mssql.Database;
using Uow.Mssql.Database.UnitOfWork;
using Uow.Mssql.Tests.Fakes;

namespace Uow.Mssql.Tests.Infrastructure
{
    public class DatabaseFixture : IDisposable
    {
        private readonly ICreateUnitOfWork _createUnitOfWork;
        private readonly TestDatabaseContext _testDatabaseContext;
        private readonly FakeTransactionalEventPublisherFactory _fakeTransactionalEventPublisherFactory;

        public IGetUnitOfWork GetUnitOfWork { get; }
        public FakeEventPublisher EventPublisher { get => _fakeTransactionalEventPublisherFactory.CurrentEventPublisher; }

        public DatabaseFixture()
        {
            _testDatabaseContext = new TestDatabaseContext();
            _testDatabaseContext.InitializeTestDatabase();
            var connectionString = _testDatabaseContext.ConnectionString;

            var sqlSettings = new SqlSettings { ConnectionString = connectionString };

            _fakeTransactionalEventPublisherFactory = new FakeTransactionalEventPublisherFactory();
            var unitOfWorkContext = new UnitOfWorkContext(sqlSettings, _fakeTransactionalEventPublisherFactory);
            _createUnitOfWork = unitOfWorkContext;
            GetUnitOfWork = unitOfWorkContext;
        }

        public (IDbConnection connection, IDbTransaction transaction) GetCurrentConnection() => GetUnitOfWork.GetConnection();
        public IUnitOfWork CreateUnitOfWork() => _createUnitOfWork.Create();

        public void Dispose()
        {
            _testDatabaseContext.Dispose();
        }
    }
}
