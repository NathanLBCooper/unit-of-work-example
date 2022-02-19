using Shouldly;
using System.Threading.Tasks;
using Uow.SqliteWithEvents.Database;
using Uow.SqliteWithEvents.Tests.Infrastructure;
using Xunit;

namespace Uow.SqliteWithEvents.Tests
{
    public class EntityRepositoryTest : RepositoryTest
    {
        private readonly EntityRepository _repository;

        public EntityRepositoryTest()
        {
            _repository = new EntityRepository(Fixture.GetUnitOfWork);
        }

        [Fact]
        public void Create_should_create_entity()
        {
            var value = 456;

            var id = _repository.Create(value);

            var result = _repository.GetOrDefault(id);
            result.Id.ShouldBe(id);
            result.Value.ShouldBe(value);
        }

        [Fact]
        public void Create_should_publish_event()
        {
            var id = _repository.Create(789);

            var @event = Fixture.CurrentEventPublisher.Staged.ShouldHaveSingleItem();
            @event.Id.ShouldBe(id);
        }
    }
}
