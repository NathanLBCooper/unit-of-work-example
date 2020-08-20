using System.Data.SQLite;

namespace UnitOfWork.SQLite
{
    public interface IConnectionContext
    {
        SQLiteConnection GetConnection();
    }
}
