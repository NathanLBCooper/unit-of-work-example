using System.Data;

namespace Uow.Postgresql.Database.UnitOfWork
{
    public interface IGetUnitOfWork
    {
        IDbConnection GetConnection();
    }
}