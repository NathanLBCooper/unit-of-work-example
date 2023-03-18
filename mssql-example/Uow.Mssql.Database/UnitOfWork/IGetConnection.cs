using System.Data;

namespace Uow.Mssql.Database.UnitOfWork;

public interface IGetConnection
{
    (IDbConnection connection, IDbTransaction transaction) GetConnection();
}
