using System.Threading.Tasks;
using Shouldly;
using Uow.Mssql.Database;
using Uow.Mssql.Tests.Infrastructure;
using Xunit;

namespace Uow.Mssql.Tests
{
    [Collection("DatabaseTest")]
    public class Examples
    {
        private readonly DatabaseFixture _fixture;
        private readonly EntityRepository _repository;

        public Examples(DatabaseFixture fixture)
        {
            _fixture = fixture;
            _repository = new EntityRepository(fixture.GetUnitOfWork);
        }

        /**
         * This test opens a transaction, inserts and entity, and commits the unit of work.
         * Then it opens another transaction and expects the entity to still be there.
         */
        [Fact]
        public async Task Commit_something_and_its_saved()
        {
            var value = 567;
            int id;

            using (var uow = _fixture.CreateUnitOfWork())
            {
                id = await _repository.Create(value);
                await uow.CommitAsync();
            }

            // todo test staged and publish events

            using (_fixture.CreateUnitOfWork())
            {
                var entity = await _repository.GetOrDefault(id);
                entity.ShouldNotBeNull();
                entity.Id.ShouldBe(id);
                entity.Value.ShouldBe(value);
            }
        }

        /**
         * This test opens a transaction, inserts and entity, and then rolls back the unit of work.
         * Then it opens another transaction and expects the entity not to be there.
         */
        [Fact]
        public async Task Rollback_Something_and_it_never_happened()
        {
            var value = 567;
            int id;

            using (var uow = _fixture.CreateUnitOfWork())
            {
                id = await _repository.Create(value);
                await uow.RollBackAsync();
            }

            // todo test staged and publish events

            using (_fixture.CreateUnitOfWork())
            {
                var entity = _repository.GetOrDefault(id);
                entity.ShouldBeNull();
            }
        }

        /**
         * This test opens a transaction, inserts and entity, and then neither commits nor rolls back the unit of work.
         * Then it opens another transaction and expects the entity not to be there.
         * That's because rollback is the default.
         */
        [Fact]
        public async Task Fail_to_commit_Something_and_it_never_happened()
        {
            var value = 567;
            int id;

            using (var uow = _fixture.CreateUnitOfWork())
            {
                id = await _repository.Create(value);
            }

            // todo test staged and publish events

            using (_fixture.CreateUnitOfWork())
            {
                var entity = _repository.GetOrDefault(id);
                entity.ShouldBeNull();
            }
        }
    }
}
