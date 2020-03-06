using System.Data.SQLite;

namespace UnitOfWork
{
    public interface IConnectionContext
    {
        SQLiteConnection GetConnection();
    }
}