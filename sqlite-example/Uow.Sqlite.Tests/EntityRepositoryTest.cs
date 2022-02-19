using Shouldly;
using System.Threading.Tasks;
using Uow.Sqlite.Database;
using Uow.Sqlite.Tests.Infrastructure;
using Xunit;

namespace Uow.Sqlite.Tests
{
    public class EntityRepositoryTest : RepositoryTest
    {
        private readonly EntityRepository _repository;

        public EntityRepositoryTest()
        {
            _repository = new EntityRepository(Fixture.GetUnitOfWork);
        }

        /**
         * This test tests the repository, not the unit of work.
         * Since it doesn't care about unit of work, it gives that responsibility away to RepositoryTest.
         *  RepositoryTest is written such that each test happens within an open transaction which is then is rolled back.
         *  This roll-back helps to make test isolation easy once we start using non-in-memory databases and need to share a db between tests
         */
        [Fact]
        public void Create_should_create_entity()
        {
            var value = 456;

            var id = _repository.Create(value);

            var result = _repository.GetOrDefault(id);
            result.Id.ShouldBe(id);
            result.Value.ShouldBe(value);
        }
    }
}
