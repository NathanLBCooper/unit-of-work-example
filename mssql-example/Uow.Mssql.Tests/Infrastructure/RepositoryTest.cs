using System;
using Uow.Mssql.Database.UnitOfWork;
using Xunit;

namespace Uow.Mssql.Tests.Infrastructure;

[Collection("DatabaseTest")]
public class RepositoryTest : IDisposable
{
    protected readonly DatabaseFixture Fixture;
    private readonly IUnitOfWork _unitOfWork;

    public RepositoryTest(DatabaseFixture fixture)
    {
        Fixture = fixture;
        _unitOfWork = fixture.CreateUnitOfWork();
    }

    public void Dispose()
    {
        _unitOfWork.RollBackAsync().GetAwaiter().GetResult();
        _unitOfWork?.Dispose();
    }
}
