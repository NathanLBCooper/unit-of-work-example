using System;
using System.Data.SQLite;
using System.Threading.Tasks;

namespace UnitOfWork
{
    public class UnitOfWork : IDisposable
    {

        private readonly SQLiteTransaction _transaction;
        public SQLiteConnection Connection { get; }

        public bool IsDisposed { get; private set; } = false;

        public UnitOfWork(SQLiteConnection connection)
        {
            Connection = connection;
            _transaction = Connection.BeginTransaction();
        }

        public async Task RollBackAsync()
        {
            await _transaction.RollbackAsync();
        }

        public async Task CommitAsync()
        {
            await _transaction.CommitAsync();
        }

        public void Dispose()
        {
            _transaction?.Dispose();

            IsDisposed = true;
        }
    }
}
