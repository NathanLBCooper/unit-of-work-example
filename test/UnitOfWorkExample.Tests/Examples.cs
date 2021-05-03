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
        public void Commit_something_and_its_saved()
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
        public void Rollback_Something_and_it_never_happened()
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

        [Fact]
        public void Fail_to_commit_Something_and_it_never_happened()
        {
            var expectedEntity = new Entity { Id = null, Value = 10 };

            using (var uow = _context.Create())
            {
                expectedEntity.Id = _repo.Create(expectedEntity.Value);
            }

            using (_context.Create())
            {
                var entity = _repo.GetOrDefault(expectedEntity.Id.Value);
                entity.Should().BeNull();
            }
        }

        /**
         * QUESTIONS
         *
         * - The context takes an open connection. What if don't want an open connection for the lifetime of my request, application etc?
         *      -> Usually I would have UnitOfWork take a `string connectionString` and have it manage the connection.
         *          I haven't here because in these SQLite tests the Database only exists as long as the connection is open
         *
         * - Wouldn't async methods be better?
         *      -> Yes. Just not for SQLite.
         *
         * - What if I had, for example, an EntityCreated event I needed to publish only if I make changes and Commit them. How would I make that work with the UnitOfWork?
         *      -> Either use or create some kind of transactional event publisher
         *              At its most basic, it needs a method to collect up the events you want to send and it needs a Commit() method that actually sends the events.
         *              Call the commit method from the UnitOfWork Commit()
         *          You can put anything you want in your UOW really. Multiple databases, event publishing, hangfire scheduling etc. Whatever you think belongs together conceptually as a unit of work.
         *
         * - Can I have an example of how I would add this to my IOC container?
         *      -> Yes, for SimpleInjector:
         *              var uowRegistration = Lifestyle.Scoped.CreateRegistration<UnitOfWorkContext>(
         *              () => new UnitOfWorkContext(container.GetInstance<SqlSettings>().ConnectionString), container);
         *              container.AddRegistration(typeof(ICreateUnitOfWork), uowRegistration);
         *              container.AddRegistration(typeof(IGetUnitOfWork), uowRegistration);
         *              container.Register<EntityRepository>(Lifestyle.Scoped);
         *
         * - What if I have multiple databases with multiple different UnitOfWork I want to be distinct and manage separately. How do I do that without duplicating this code?
         *      -> I've tried just making everything pointlessly generic. eg IUnitOfWork<TConnection>, where TConnection is an empty type named after the database.
         *          That way I can't mix up different UOWs or UOWContexts because the compiler won't let me.
         */

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
