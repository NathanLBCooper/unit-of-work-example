using System.Data;

namespace Uow.SqliteWithEvents.Example.UnitOfWork;

public interface IGetConnection
{
    IDbConnection GetConnection();
    IEventPublisher GetEventPublisher();
}
