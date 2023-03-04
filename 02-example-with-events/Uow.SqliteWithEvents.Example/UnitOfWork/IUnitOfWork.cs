using System;

namespace Uow.SqliteWithEvents.Example.UnitOfWork;

public interface IUnitOfWork : IDisposable
{
    void RollBack();
    void Commit();
    bool IsDisposed { get; }
}
