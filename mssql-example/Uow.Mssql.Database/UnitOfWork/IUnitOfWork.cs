using System;
using System.Threading.Tasks;

namespace Uow.Mssql.Database.UnitOfWork;

public interface IUnitOfWork : IDisposable
{
    Task RollBackAsync();
    Task CommitAsync();
    bool IsDisposed { get; }
}
