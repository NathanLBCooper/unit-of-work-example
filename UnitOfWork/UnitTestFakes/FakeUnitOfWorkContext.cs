using System;
using UnitOfWork.SQLite;

namespace UnitOfWork.UnitTestFakes
{
    public class FakeUnitOfWorkContext : IUnitOfWorkContext
    {
        private FakeUnitOfWork _unitOfWork;
        private FakeDbConnection _connection;

        private bool IsUnitOfWorkOpen => !(_unitOfWork == null || _unitOfWork.IsDisposed);

        public FakeDbConnection GetConnection()
        {
            if (!IsUnitOfWorkOpen)
            {
                throw new InvalidOperationException(
                    "There is not current unit of work open. Call BeginTransaction first");
            }

            return _connection;
        }

        public IUnitOfWork Create()
        {
            if (IsUnitOfWorkOpen)
            {
                throw new InvalidOperationException(
                    "Cannot begin a transaction before the unit of work from the last one is disposed");
            }

            _connection = new FakeDbConnection();
            _unitOfWork = new FakeUnitOfWork(_connection);
            return _unitOfWork;
        }
    }
}
