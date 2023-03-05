using Xunit;

namespace Uow.EntityFramework.Tests.Infrastructure;

[CollectionDefinition("DatabaseTest")]
public class DatabaseCollection : ICollectionFixture<DatabaseFixture>
{

}
