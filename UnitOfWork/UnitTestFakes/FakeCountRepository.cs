using System.Threading.Tasks;

namespace UnitOfWork.UnitTestFakes
{
    public class FakeCountRepository
    {
        private FakeUnitOfWorkContext _context;
        private const string CountKey = "FakeCountRepository.Count";

        public int LastCommitedCount { get; private set; } = 0;

        public FakeCountRepository(FakeUnitOfWorkContext context)
        {
            _context = context;
        }

        public Task IncrementCount()
        {
            var connection = _context.GetConnection();
            connection.EnsureObjectIsTracked(CountKey, LastCommitedCount, CountCommitted);

            var currentCount = (int)connection.ReadObject(CountKey);
            connection.WriteObject(CountKey, currentCount + 1);

            return Task.CompletedTask;
        }

        public Task<int> ReadCount()
        {
            var connection = _context.GetConnection();
            connection.EnsureObjectIsTracked(CountKey, LastCommitedCount, CountCommitted);

            var currentCount = (int)connection.ReadObject(CountKey);

            return Task.FromResult(currentCount);
        }

        private void CountCommitted(object count)
        {
            LastCommitedCount = (int) count;
        }
    }
}
