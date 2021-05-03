using System.Data.SQLite;

namespace UnitOfWorkExample.UnitOfWork
{
    public interface IGetUnitOfWork
    {
        SQLiteConnection GetConnection();
    }
}