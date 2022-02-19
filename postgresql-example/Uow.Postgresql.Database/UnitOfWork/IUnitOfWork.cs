using Npgsql;
using System;
using System.Threading.Tasks;

namespace Uow.Postgresql.Database.UnitOfWork
{
    public interface IUnitOfWork : IDisposable
    {
        Task RollBackAsync();
        Task CommitAsync();
        bool IsDisposed { get; }
        NpgsqlConnection Connection { get; }
    }
}