using System;
using System.Threading.Tasks;
using UnitOfWork.SQLite;

namespace UnitOfWork.UnitTestFakes
{
    public class FakeUnitOfWork : IUnitOfWork
    {
        public FakeDbConnection Connection { get; }
        public bool IsDisposed { get; private set; } = false;

        public FakeUnitOfWork(FakeDbConnection connection)
        {
            Connection = connection;
        }

        public Task CommitAsync()
        {
            if (IsDisposed)
            {
                throw new ObjectDisposedException(GetType().FullName);
            }

            Connection.Commit();
            return Task.CompletedTask;
        }

        public Task RollBackAsync()
        {
            if (IsDisposed)
            {
                throw new ObjectDisposedException(GetType().FullName);
            }

            return Task.CompletedTask;
        }

        public void Dispose()
        {
            IsDisposed = true;
        }
    }
}