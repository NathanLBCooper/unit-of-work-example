using System.Data.SQLite;

namespace Uow.Sqlite.Database.UnitOfWork
{
    public interface IGetUnitOfWork
    {
        SQLiteConnection GetConnection();
    }
}