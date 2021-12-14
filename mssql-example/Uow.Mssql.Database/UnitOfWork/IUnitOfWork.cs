using System;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace Uow.Mssql.Database.UnitOfWork
{
    public interface IUnitOfWork : IDisposable
    {
        Task RollBackAsync();
        Task CommitAsync();
        bool IsDisposed { get; }
        SqlTransaction Transaction { get; }
        SqlConnection Connection { get; }
        IEventPublisher EventPublisher { get; }
    }
}