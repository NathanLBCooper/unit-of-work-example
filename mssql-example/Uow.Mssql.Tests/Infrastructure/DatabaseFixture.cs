using System;
using System.Data;
using Uow.Mssql.Database;
using Uow.Mssql.Database.UnitOfWork;

namespace Uow.Mssql.Tests.Infrastructure
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

        public (IDbConnection connection, IDbTransaction transaction) GetCurrentConnection() => GetUnitOfWork.GetConnection();
        public IUnitOfWork CreateUnitOfWork() => _createUnitOfWork.Create();

        public void Dispose()
        {
            _testDatabaseContext.Dispose();
        }
    }
}
