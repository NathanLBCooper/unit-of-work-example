using Npgsql;
using System;
using System.Threading.Tasks;

namespace Uow.Postgresql.Database.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork, IDisposable
    {
        private readonly NpgsqlTransaction _transaction;
        public NpgsqlConnection Connection { get; }
        public bool IsDisposed { get; private set; } = false;

        public UnitOfWork(string connectionString)
        {
            Connection = new NpgsqlConnection(connectionString);
            Connection.Open();
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
            Connection?.Dispose();

            IsDisposed = true;
        }
    }
}
