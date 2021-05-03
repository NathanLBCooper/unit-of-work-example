using System;
using System.Data.SQLite;

namespace UnitOfWorkExample.UnitOfWork
{
    public interface IUnitOfWork : IDisposable
    {
        void RollBack();
        void Commit();
    }

    public class UnitOfWork : IUnitOfWork, IDisposable
    {
        // Note, when using anything other than SQLite I would
        // 1) I would have passed this class a `string connectionString` and have it open and dispose of the connection, rather than passing it an open connection.
        // 2) Use async methods

        private readonly SQLiteTransaction _transaction;
        public SQLiteConnection Connection { get; }

        public bool IsDisposed { get; private set; } = false;

        public UnitOfWork(SQLiteConnection connection)
        {
            Connection = connection;
            _transaction = Connection.BeginTransaction();
        }

        public void RollBack()
        {
            _transaction.Rollback();
        }

        public void Commit()
        {
            _transaction.Commit();
        }

        public void Dispose()
        {
            _transaction?.Dispose();

            IsDisposed = true;
        }
    }
}
