using System;
using System.Data.SQLite;
using Dapper;
using FluentAssertions;
using Xunit;
using UnitOfWorkExample.ExampleDb;
using UnitOfWorkExample.UnitOfWork;

namespace UnitOfWorkExample.Tests
{
    public class Examples : IDisposable
    {
        private readonly SQLiteConnection _connection;

        private readonly UnitOfWorkContext _context;
        private readonly EntityRepository _repo;

        public Examples()
        {
            _connection = SetupDatabase();
            _context = new UnitOfWorkContext(_connection);
            _repo = new EntityRepository(_context);
        }

        [Fact]
        public void Commit_something()
        {
            var expectedEntity = new Entity { Id = null, Value = 10 };

            using (var uow = _context.Create())
            {
                expectedEntity.Id = _repo.Create(expectedEntity.Value);

                uow.Commit();
            }

            using (_context.Create())
            {
                var entity = _repo.GetOrDefault(expectedEntity.Id.Value);
                entity.Should().NotBeNull();
                entity.Value.Should().Be(expectedEntity.Value);
            }
        }

        [Fact]
        public void Rollback_Something()
        {
            var expectedEntity = new Entity { Id = null, Value = 10 };

            using (var uow = _context.Create())
            {
                expectedEntity.Id = _repo.Create(expectedEntity.Value);

                uow.RollBack();
            }

            using (_context.Create())
            {
                var entity = _repo.GetOrDefault(expectedEntity.Id.Value);
                entity.Should().BeNull();
            }
        }

        private static SQLiteConnection SetupDatabase()
        {
            var connection = new SQLiteConnection("Data Source=:memory:");
            connection.Open();
            connection.Execute(
                    @"
create table Entity (
    Id integer primary key,
    Value integer not null
);");
            return connection;
        }

        public void Dispose()
        {
            _connection?.Dispose();
        }
    }
}
