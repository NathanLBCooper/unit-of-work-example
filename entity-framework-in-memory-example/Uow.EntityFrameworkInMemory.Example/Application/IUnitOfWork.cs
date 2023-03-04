using System;

namespace Uow.EntityFrameworkInMemory.Example.Application;

public interface IUnitOfWork : IDisposable
{
    void RollBack();
    void Commit();
    bool IsDisposed { get; }
}
