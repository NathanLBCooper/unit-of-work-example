using System;

namespace Uow.Sqlite.Example.Application;

public interface IUnitOfWork : IDisposable
{
    void RollBack();
    void Commit();
    bool IsDisposed { get; }
}
