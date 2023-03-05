using System;

namespace Uow.EntityFramework.Example.Application;

public interface IUnitOfWork : IDisposable
{
    void RollBack();
    void Commit();
    bool IsDisposed { get; }
}
