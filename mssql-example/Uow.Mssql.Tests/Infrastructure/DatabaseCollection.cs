using Xunit;

namespace Uow.Mssql.Tests.Infrastructure;

[CollectionDefinition("DatabaseTest")]
public class DatabaseCollection : ICollectionFixture<DatabaseFixture>
{

}
