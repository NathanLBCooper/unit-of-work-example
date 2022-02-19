using System;
using System.Data.SQLite;

namespace Uow.SqliteWithEvents.Database.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork, IDisposable
    {
        private readonly ITransactionalEventPublisher _eventPublisher;
        private readonly SQLiteTransaction _transaction;

        public SQLiteConnection Connection { get; }
        public IEventPublisher EventPublisher => _eventPublisher;

        public bool IsDisposed { get; private set; } = false;

        public UnitOfWork(SQLiteConnection connection, ITransactionalEventPublisherFactory transactionalEventPublisherFactory)
        {
            Connection = connection;
            _transaction = Connection.BeginTransaction();
            _eventPublisher = transactionalEventPublisherFactory.Create();
        }

        public void RollBack()
        {
            _transaction.Rollback();
        }

        public void Commit()
        {
            _transaction.Commit();
            _eventPublisher.Commit();
        }

        public void Dispose()
        {
            _transaction?.Dispose();

            IsDisposed = true;
        }
    }
}
