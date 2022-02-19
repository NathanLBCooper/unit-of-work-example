using Xunit;

namespace Uow.Postgresql.Tests.Infrastructure
{
    [CollectionDefinition("DatabaseTest")]
    public class DatabaseCollection : ICollectionFixture<DatabaseFixture>
    {

    }
}
