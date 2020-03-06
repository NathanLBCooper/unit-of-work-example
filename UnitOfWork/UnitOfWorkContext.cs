using System;
using System.Data.SQLite;

namespace UnitOfWork
{
    // Put this in your IOC at the scope that's appropriate (Singleton, PerRequest, PerThread etc)
    public class UnitOfWorkContext : IUnitOfWorkContext, IConnectionContext
    {
        private readonly SQLiteConnection _connection;
        private UnitOfWork _unitOfWork;

        private bool IsUnitOfWorkOpen => !(_unitOfWork == null || _unitOfWork.IsDisposed);

        public UnitOfWorkContext(SQLiteConnection connection)
        {
            _connection = connection;
        }

        public SQLiteConnection GetConnection()
        {
            if (!IsUnitOfWorkOpen)
            {
                throw new InvalidOperationException(
                    "There is not current unit of work from which to get a connection. Call BeginTransaction first");
            }

            return _unitOfWork.Connection;
        }

        public UnitOfWork Create()
        {
            if (IsUnitOfWorkOpen)
            {
                throw new InvalidOperationException(
                    "Cannot begin a transaction before the unit of work from the last one is disposed");
            }

            _unitOfWork = new UnitOfWork(_connection);
            return _unitOfWork;
        }
    }
}
