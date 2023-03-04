using Dapper;
using Uow.SqliteWithEvents.Database;
using Uow.SqliteWithEvents.Example.UnitOfWork;

namespace Uow.SqliteWithEvents.Example;

public class EntityRepository
{
    private readonly IGetConnection _getConnection;

    public EntityRepository(IGetConnection getConnection)
    {
        _getConnection = getConnection;
    }

    public int Create(int value)
    {
        var id = _getConnection.GetConnection().QuerySingle<int>(
            @"
insert into Entity (Value) values (@value);
select last_insert_rowid();
", new { value });

        _getConnection.GetEventPublisher().PublishEvent(new EntityEvent { Id = id });

        return id;
    }

    public Entity GetOrDefault(int id)
    {
        return _getConnection.GetConnection().QuerySingleOrDefault<Entity>(
            @"
select * from Entity where Id = @id
", new { id });
    }
}
