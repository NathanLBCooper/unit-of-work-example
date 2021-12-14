using System;

namespace Uow.Sqlite.Database.UnitOfWork
{
    public interface IUnitOfWork : IDisposable
    {
        void RollBack();
        void Commit();
    }
}