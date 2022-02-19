using System.Data.SQLite;

namespace Uow.SqliteWithEvents.Database.UnitOfWork
{
    public interface IGetUnitOfWork
    {
        SQLiteConnection GetConnection();
        IEventPublisher GetEventPublisher();
    }
}