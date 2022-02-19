using System.Threading.Tasks;
using Shouldly;
using Uow.Postgresql.Database;
using Uow.Postgresql.Tests.Infrastructure;
using Xunit;

namespace Uow.Postgresql.Tests
{
    [Collection("DatabaseTest")]
    public class UnitOfWorkTest
    {
        private readonly DatabaseFixture _fixture;
        private readonly EntityRepository _repository;

        public UnitOfWorkTest(DatabaseFixture fixture)
        {
            _fixture = fixture;
            _repository = new EntityRepository(fixture.GetUnitOfWork);
        }

        [Fact]
        public async Task Creating_an_entity_and_commiting_saves_to_the_db()
        {
            var value = 567;
            int id;

            using (var uow = _fixture.CreateUnitOfWork())
            {
                id = await _repository.Create(value);
                await uow.CommitAsync();
            }

            using (_fixture.CreateUnitOfWork())
            {
                var entity = await _repository.GetOrDefault(id);
                entity.ShouldNotBeNull();
                entity.Id.ShouldBe(id);
                entity.Value.ShouldBe(value);
            }
        }

        [Fact]
        public async Task Creating_an_entity_and_rolling_back_saves_nothing()
        {
            var value = 567;
            int id;

            using (var uow = _fixture.CreateUnitOfWork())
            {
                id = await _repository.Create(value);
                await uow.RollBackAsync();
            }

            using (_fixture.CreateUnitOfWork())
            {
                var entity = await _repository.GetOrDefault(id);
                entity.ShouldBeNull();
            }
        }

        [Fact]
        public async Task Creating_an_entity_and_not_commiting_saves_nothing()
        {
            var value = 567;
            int id;

            using (var uow = _fixture.CreateUnitOfWork())
            {
                id = await _repository.Create(value);
            }

            using (_fixture.CreateUnitOfWork())
            {
                var entity = await _repository.GetOrDefault(id);
                entity.ShouldBeNull();
            }
        }
    }
}
