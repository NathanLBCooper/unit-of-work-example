using System.Data.SQLite;

namespace UnitOfWork
{
    public interface IGetUnitOfWork
    {
        SQLiteConnection GetConnection();
    }
}