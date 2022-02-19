using System;
using Uow.Sqlite.Database.UnitOfWork;

namespace Uow.Sqlite.Tests.Infrastructure
{
    public class DatabaseFixture : IDisposable
    {
        private readonly TestDatabaseContext _testDatabaseContext;

        public ICreateUnitOfWork CreateUnitOfWork;
        public IGetUnitOfWork GetUnitOfWork { get; }

        public DatabaseFixture()
        {
            _testDatabaseContext = new TestDatabaseContext();
            var connection = _testDatabaseContext.Connection;

            var unitOfWorkContext = new UnitOfWorkContext(connection);
            CreateUnitOfWork = unitOfWorkContext;
            GetUnitOfWork = unitOfWorkContext;
        }

        public void Dispose()
        {
            _testDatabaseContext.Dispose();
        }
    }
}
