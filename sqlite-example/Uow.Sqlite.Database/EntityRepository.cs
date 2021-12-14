using Dapper;
using Uow.Sqlite.Database.UnitOfWork;

namespace Uow.Sqlite.Database
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
            return _getUnitOfWork.GetConnection().QuerySingle<int>(
                @"
insert into Entity (Value) values (@value);
select last_insert_rowid();
", new { value });
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
