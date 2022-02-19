using Dapper;
using Uow.SqliteWithEvents.Database.UnitOfWork;

namespace Uow.SqliteWithEvents.Database
{
    public class EntityRepository
    {
        private readonly IGetUnitOfWork _getUnitOfWork;

        public EntityRepository(IGetUnitOfWork getUnitOfWork)
        {
            _getUnitOfWork = getUnitOfWork;
        }

        public int Create(int value)
        {
            var id = _getUnitOfWork.GetConnection().QuerySingle<int>(
                @"
insert into Entity (Value) values (@value);
select last_insert_rowid();
", new { value });

            _getUnitOfWork.GetEventPublisher().PublishEvent(new EntityEvent { Id = id });

            return id;
        }

        public Entity GetOrDefault(int id)
        {
            return _getUnitOfWork.GetConnection().QuerySingleOrDefault<Entity>(
                @"
select * from Entity where Id = @id
", new { id });
        }
    }
}
