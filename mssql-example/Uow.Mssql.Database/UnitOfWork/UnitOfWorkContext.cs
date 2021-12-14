using System;
using System.Data;

namespace Uow.Mssql.Database.UnitOfWork
{
    public class UnitOfWorkContext : ICreateUnitOfWork, IGetUnitOfWork
    {
        private readonly ITransactionalEventPublisherFactory _transactionalEventPublisherFactory;
        private readonly string _connectionString;
        private IUnitOfWork _unitOfWork;

        private bool IsUnitOfWorkOpen => !(_unitOfWork == null || _unitOfWork.IsDisposed);

        public UnitOfWorkContext(SqlSettings sqlSettings, ITransactionalEventPublisherFactory transactionalEventPublisherFactory)
        {
            _transactionalEventPublisherFactory = transactionalEventPublisherFactory;
            _connectionString = sqlSettings.ConnectionString;
        }

        public (IDbConnection connection, IDbTransaction transaction) GetConnection()
        {
            if (!IsUnitOfWorkOpen)
            {
                throw new InvalidOperationException(
                    "There is not current unit of work from which to get a connection. Call Create first");
            }

            return (_unitOfWork.Connection, _unitOfWork.Transaction);
        }

        public IEventPublisher GetEventPublisher()
        {
            if (!IsUnitOfWorkOpen)
            {
                throw new InvalidOperationException(
                    "There is not current unit of work from which to get a connection. Call Create first");
            }

            return _unitOfWork.EventPublisher;
        }

        public IUnitOfWork Create()
        {
            if (IsUnitOfWorkOpen)
            {
                throw new InvalidOperationException(
                    "Cannot begin a transaction before the unit of work from the last one is disposed");
            }

            _unitOfWork = new UnitOfWork(_connectionString, _transactionalEventPublisherFactory);
            return _unitOfWork;
        }
    }
}
