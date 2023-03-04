using Dapper;
using Uow.Sqlite.Example.Application;

namespace Uow.Sqlite.Example.Storage;

public class EntityRepository : IEntityRepository
{
    private readonly IGetConnection _getConnection;

    public EntityRepository(IGetConnection getConnection)
    {
        _getConnection = getConnection;
    }

    public int Create(int value)
    {
        return _getConnection.GetConnection().QuerySingle<int>(
            @"
insert into Entity (Value) values (@value);
select last_insert_rowid();
", new { value });
    }

    public Entity? GetOrDefault(int id)
    {
        return _getConnection.GetConnection().QuerySingleOrDefault<Entity>(
            @"
select * from Entity where Id = @id
", new { id });
    }
}
