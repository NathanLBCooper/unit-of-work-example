using System;
using Uow.EntityFramework.Example.Application;
using Xunit;

namespace Uow.EntityFramework.Tests.Infrastructure;

[Collection("DatabaseTest")]
public class RepositoryTest : IDisposable
{
    protected readonly DatabaseFixture Fixture;
    private readonly IUnitOfWork _unitOfWork;

    public RepositoryTest(DatabaseFixture fixture)
    {
        Fixture = fixture;
        _unitOfWork = fixture.CreateUnitOfWork.Create();
    }

    public void Dispose()
    {
        _unitOfWork.RollBack();
        _unitOfWork?.Dispose();
    }
}
