using System;
using System.Data;
using Uow.Postgresql.Database;
using Uow.Postgresql.Database.UnitOfWork;

namespace Uow.Postgresql.Tests.Infrastructure
{
    public class DatabaseFixture : IDisposable
    {
        private readonly ICreateUnitOfWork _createUnitOfWork;
        private readonly TestDatabaseContext _testDatabaseContext;

        public IGetUnitOfWork GetUnitOfWork { get; }

        public DatabaseFixture()
        {
            _testDatabaseContext = new TestDatabaseContext();
            _testDatabaseContext.InitializeTestDatabase();
            var connectionString = _testDatabaseContext.ConnectionString;

            var sqlSettings = new SqlSettings { ConnectionString = connectionString };

            var unitOfWorkContext = new UnitOfWorkContext(sqlSettings);
            _createUnitOfWork = unitOfWorkContext;
            GetUnitOfWork = unitOfWorkContext;
        }

        public IDbConnection GetCurrentConnection() => GetUnitOfWork.GetConnection();
        public IUnitOfWork CreateUnitOfWork() => _createUnitOfWork.Create();

        public void Dispose()
        {
            _testDatabaseContext.Dispose();
        }
    }
}
