using System.Data;

namespace Uow.Sqlite.Example.Storage;

public interface IGetConnection
{
    IDbConnection GetConnection();
}
