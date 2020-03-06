using System.Data.SQLite;
using System.Threading.Tasks;
using Dapper;
using FluentAssertions;

namespace UnitOfWork
{
    public class Program
    {
        public static void Main(string[] args)
        {
            // Using one open connection for everything because of in memory db. Real use of this pattern may differ.
            using (var connection = new SQLiteConnection("Data Source=:memory:"))
            {
                connection.Open();
                Setup(connection);
                var context = new UnitOfWorkContext(connection);
                var repo = new EntityRepository(context);

                CreateSomething(repo, context).ConfigureAwait(false).GetAwaiter().GetResult();
                RollSomethingBack(repo, context).ConfigureAwait(false).GetAwaiter().GetResult();
            }
        }

        private static async Task CreateSomething(EntityRepository repo, IUnitOfWorkContext context)
        {
            var expectedEntity = new Entity {Id = null, Value = 10};

            using (var uow = context.Create())
            {
                expectedEntity.Id = await repo.CreateAsync(expectedEntity.Value);
                await uow.CommitAsync();
            }

            using (context.Create())
            {
                var entity = await repo.GetOrDefaultAsync(expectedEntity.Id.Value);
                entity.Should().NotBeNull();
                entity.Value.Should().Be(expectedEntity.Value);
            }
        }

        private static async Task RollSomethingBack(EntityRepository repo, IUnitOfWorkContext context)
        {
            int entityId;
            using (var uow = context.Create())
            {
                entityId = await repo.CreateAsync(101);
                await uow.RollBackAsync(); // or fail to commit, same thing
            }

            using (context.Create())
            {
                var entity = await repo.GetOrDefaultAsync(entityId);
                entity.Should().BeNull();
            }
        }

        private static void Setup(SQLiteConnection connection)
        {
            connection.Execute(
                    @"
create table Entity (
    Id integer primary key,
    Value integer not null
);");
        }
    }
}
