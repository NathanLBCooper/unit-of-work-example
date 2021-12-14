using System;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace Uow.Mssql.Database.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork, IDisposable
    {
        private readonly ITransactionalEventPublisher _eventPublisher;

        public SqlTransaction Transaction { get; }
        public SqlConnection Connection { get; }
        public IEventPublisher EventPublisher => _eventPublisher;

        public bool IsDisposed { get; private set; } = false;

        public UnitOfWork(string connectionString, ITransactionalEventPublisherFactory transactionalEventPublisherFactory)
        {
            Connection = new SqlConnection(connectionString);
            Connection.Open();
            Transaction = Connection.BeginTransaction();
            _eventPublisher = transactionalEventPublisherFactory.Create();
        }

        public async Task RollBackAsync()
        {
            await Transaction.RollbackAsync();
        }

        public async Task CommitAsync()
        {
            await Transaction.CommitAsync();
            _eventPublisher.Commit();
        }

        public void Dispose()
        {
            Transaction?.Dispose();
            Connection?.Dispose();

            IsDisposed = true;
        }
    }
}
