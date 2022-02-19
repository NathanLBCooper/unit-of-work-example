using System;
using System.Data;

namespace Uow.Postgresql.Database.UnitOfWork
{
    public class UnitOfWorkContext : ICreateUnitOfWork, IGetUnitOfWork
    {
        private readonly string _connectionString;
        private IUnitOfWork _unitOfWork;

        private bool IsUnitOfWorkOpen => !(_unitOfWork == null || _unitOfWork.IsDisposed);

        public UnitOfWorkContext(SqlSettings sqlSettings)
        {
            _connectionString = sqlSettings.ConnectionString;
        }

        public IDbConnection GetConnection()
        {
            if (!IsUnitOfWorkOpen)
            {
                throw new InvalidOperationException(
                    "There is not current unit of work from which to get a connection. Call Create first");
            }

            return _unitOfWork.Connection;
        }

        public IUnitOfWork Create()
        {
            if (IsUnitOfWorkOpen)
            {
                throw new InvalidOperationException(
                    "Cannot begin a transaction before the unit of work from the last one is disposed");
            }

            _unitOfWork = new UnitOfWork(_connectionString);
            return _unitOfWork;
        }
    }
}
