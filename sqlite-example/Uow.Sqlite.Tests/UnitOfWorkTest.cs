using System;
using Shouldly;
using Uow.Sqlite.Database;
using Uow.Sqlite.Database.UnitOfWork;
using Uow.Sqlite.Tests.Infrastructure;
using Xunit;

namespace Uow.Sqlite.Tests
{
    public class UnitOfWorkTest : IDisposable
    {
        private readonly DatabaseFixture _fixture = new DatabaseFixture();
        private readonly EntityRepository _repository;
        private readonly ICreateUnitOfWork _createUnitOfWork;

        public UnitOfWorkTest()
        {
            _repository = new EntityRepository(_fixture.GetUnitOfWork);
            _createUnitOfWork = _fixture.CreateUnitOfWork;
        }


        /**
         * This test opens a transaction, inserts and entity, and commits the unit of work.
         * Then it opens another transaction and expects the entity to still be there.
         */
        [Fact]
        public void Creating_an_entity_and_commiting_saves_to_the_db()
        {
            var expectedEntity = new Entity { Id = null, Value = 10 };

            using (var uow = _createUnitOfWork.Create())
            {
                expectedEntity.Id = _repository.Create(expectedEntity.Value);

                uow.Commit();
            }

            using (_createUnitOfWork.Create())
            {
                var entity = _repository.GetOrDefault(expectedEntity.Id.Value);
                entity.ShouldNotBeNull();
                entity.Value.ShouldBe(expectedEntity.Value);
            }
        }

        /**
         * This test opens a transaction, inserts and entity, and then rolls back the unit of work.
         * Then it opens another transaction and expects the entity not to be there.
         */
        [Fact]
        public void Creating_an_entity_and_rolling_back_saves_nothing()
        {
            var expectedEntity = new Entity { Id = null, Value = 10 };

            using (var uow = _createUnitOfWork.Create())
            {
                expectedEntity.Id = _repository.Create(expectedEntity.Value);

                uow.RollBack();
            }

            using (_createUnitOfWork.Create())
            {
                var entity = _repository.GetOrDefault(expectedEntity.Id.Value);
                entity.ShouldBeNull();
            }
        }

        /**
         * This test opens a transaction, inserts and entity, and then neither commits nor rolls back the unit of work.
         * Then it opens another transaction and expects the entity not to be there.
         * That's because rollback is the default.
         */
        [Fact]
        public void Creating_an_entity_and_not_commiting_saves_nothing()
        {
            var expectedEntity = new Entity { Id = null, Value = 10 };

            using (var uow = _createUnitOfWork.Create())
            {
                expectedEntity.Id = _repository.Create(expectedEntity.Value);
            }

            using (_createUnitOfWork.Create())
            {
                var entity = _repository.GetOrDefault(expectedEntity.Id.Value);
                entity.ShouldBeNull();
            }
        }

        public void Dispose()
        {
            _fixture?.Dispose();
        }
    }
}
