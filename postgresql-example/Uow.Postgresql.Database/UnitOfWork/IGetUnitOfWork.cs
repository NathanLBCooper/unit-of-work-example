using System.Data;

namespace Uow.Postgresql.Database.UnitOfWork;

public interface IGetConnection
{
    IDbConnection GetConnection();
}
