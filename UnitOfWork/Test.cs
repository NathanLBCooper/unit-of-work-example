using System;
using System.Threading.Tasks;
using FluentAssertions;
using UnitOfWork.UnitTestFakes;
using Xunit;

namespace UnitOfWork
{
    public class Test
    {
        [Fact]
        public async Task increment_something()
        {
            var context = new FakeUnitOfWorkContext();
            var repo = new FakeCountRepository(context);

            using (var uow = context.Create())
            {
                await repo.IncrementCount();
                await uow.CommitAsync();
            }

            repo.LastCommitedCount.Should().Be(1);
        }

        [Fact]
        public void forgot_to_open_a_uow()
        {
            // This is also an easy mistake to make that it would be better to catch in tests
            var repo = new FakeCountRepository(new FakeUnitOfWorkContext());

            Action write = () => repo.IncrementCount();
            Action read = () => repo.ReadCount();

            write.Should().Throw<InvalidOperationException>();
            read.Should().Throw<InvalidOperationException>();
        }

        [Fact]
        public async Task roll_something_back()
        {
            // I want a fake that has rollback
            var context = new FakeUnitOfWorkContext();
            var repo = new FakeCountRepository(context);

            using (var uow = context.Create())
            {
                await repo.IncrementCount();
                // because missing the commit is a common mistake
            }

            repo.LastCommitedCount.Should().Be(0);
        }

        [Fact]
        public async Task transactions_should_seem_real()
        {
            var context = new FakeUnitOfWorkContext();
            var repo = new FakeCountRepository(context);

            using (var uow = context.Create())
            {
                await repo.IncrementCount();
                (await repo.ReadCount()).Should().Be(1);
                await repo.IncrementCount();
                (await repo.ReadCount()).Should().Be(2);
            }

            repo.LastCommitedCount.Should().Be(0);
        }
    }
}
