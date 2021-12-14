using System;
using System.Data.SQLite;
using Dapper;
using Shouldly;
using Uow.Sqlite.Database;
using Uow.Sqlite.Database.UnitOfWork;
using Xunit;

namespace Uow.Sqlite.Tests
{
    public class Examples : IDisposable
    {
        private readonly SQLiteConnection _connection;

        private readonly UnitOfWorkContext _context;
        private readonly EntityRepository _repository;

        public Examples()
        {
            _connection = TestDatabase.CreateConnectionAndSetupDatabase();
            _context = new UnitOfWorkContext(_connection);
            _repository = new EntityRepository(_context);
        }

        /**
         * This test opens a transaction, inserts and entity, and commits the unit of work.
         * Then it opens another transaction and expects the entity to still be there.
         */
        [Fact]
        public void Commit_something_and_its_saved()
        {
            var expectedEntity = new Entity { Id = null, Value = 10 };

            using (var uow = _context.Create())
            {
                expectedEntity.Id = _repository.Create(expectedEntity.Value);

                uow.Commit();
            }

            using (_context.Create())
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
        public void Rollback_Something_and_it_never_happened()
        {
            var expectedEntity = new Entity { Id = null, Value = 10 };

            using (var uow = _context.Create())
            {
                expectedEntity.Id = _repository.Create(expectedEntity.Value);

                uow.RollBack();
            }

            using (_context.Create())
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
        public void Fail_to_commit_Something_and_it_never_happened()
        {
            var expectedEntity = new Entity { Id = null, Value = 10 };

            using (var uow = _context.Create())
            {
                expectedEntity.Id = _repository.Create(expectedEntity.Value);
            }

            using (_context.Create())
            {
                var entity = _repository.GetOrDefault(expectedEntity.Id.Value);
                entity.ShouldBeNull();
            }
        }

        public void Dispose()
        {
            _connection?.Dispose();
        }
    }
}
