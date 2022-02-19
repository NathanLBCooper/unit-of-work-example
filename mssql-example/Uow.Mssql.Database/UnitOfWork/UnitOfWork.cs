using System;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace Uow.Mssql.Database.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork, IDisposable
    {
        public SqlTransaction Transaction { get; }
        public SqlConnection Connection { get; }
        public bool IsDisposed { get; private set; } = false;

        public UnitOfWork(string connectionString)
        {
            Connection = new SqlConnection(connectionString);
            Connection.Open();
            Transaction = Connection.BeginTransaction();
        }

        public async Task RollBackAsync()
        {
            await Transaction.RollbackAsync();
        }

        public async Task CommitAsync()
        {
            await Transaction.CommitAsync();
        }

        public void Dispose()
        {
            Transaction?.Dispose();
            Connection?.Dispose();

            IsDisposed = true;
        }
    }
}
