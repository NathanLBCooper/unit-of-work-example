using System.Data;

namespace Uow.Mssql.Database.UnitOfWork
{
    public interface IGetUnitOfWork
    {
        (IDbConnection connection, IDbTransaction transaction) GetConnection();
        IEventPublisher GetEventPublisher();
    }
}