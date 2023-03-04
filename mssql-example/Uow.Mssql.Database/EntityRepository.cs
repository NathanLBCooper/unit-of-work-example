using System.Threading.Tasks;
using Dapper;
using Uow.Mssql.Database.UnitOfWork;

namespace Uow.Mssql.Database;

public class EntityRepository
{
    private readonly IGetConnection _getConnection;

    public EntityRepository(IGetConnection getConnection)
    {
        _getConnection = getConnection;
    }

    public async Task<int> Create(int value)
    {
        var (connection, transaction) = _getConnection.GetConnection();

        var query = @"
insert into Entity (Value) values (@value);
select SCOPE_IDENTITY();
";

        return await connection.QuerySingleAsync<int>(query, new { value }, transaction);
    }

    public async Task<Entity> GetOrDefault(int id)
    {
        var (connection, transaction) = _getConnection.GetConnection();

        var query = @"
select * from Entity where Id = @id;
";

        return await connection.QuerySingleOrDefaultAsync<Entity>(query, new { id }, transaction);
    }
}
