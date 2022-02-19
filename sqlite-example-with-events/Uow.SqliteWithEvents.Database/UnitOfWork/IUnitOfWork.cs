using System;
using System.Data.SQLite;

namespace Uow.SqliteWithEvents.Database.UnitOfWork
{
    public interface IUnitOfWork : IDisposable
    {
        void RollBack();
        void Commit();
        IEventPublisher EventPublisher { get; }
        SQLiteConnection Connection { get; }
        bool IsDisposed { get; }
    }
}