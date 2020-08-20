using System;
using System.Threading.Tasks;

namespace UnitOfWork.SQLite
{
    public interface IUnitOfWork : IDisposable
    {
        Task RollBackAsync();
        Task CommitAsync();
    }
}
