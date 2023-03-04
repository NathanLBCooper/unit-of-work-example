using System;
using Uow.Sqlite.Example.Application;

namespace Uow.Sqlite.Tests.Infrastructure;

public class RepositoryTest : IDisposable
{
    protected readonly DatabaseFixture Fixture = new();
    private readonly IUnitOfWork _unitOfWork;

    public RepositoryTest()
    {
        _unitOfWork = Fixture.CreateUnitOfWork.Create();
    }

    public void Dispose()
    {
        _unitOfWork.RollBack();
        _unitOfWork?.Dispose();
        Fixture?.Dispose();
    }
}
