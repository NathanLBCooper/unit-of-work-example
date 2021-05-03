using Dapper;
using UnitOfWorkExample.UnitOfWork;

namespace UnitOfWorkExample.ExampleDb
{
    public class EntityRepository
    {
        private readonly IGetUnitOfWork _context;

        public EntityRepository(IGetUnitOfWork context)
        {
            _context = context;
        }

        public int Create(int value)
        {
            return _context.GetConnection().QuerySingle<int>(
                @"
insert into Entity (Value) values (@value);
select last_insert_rowid();
", new { value });
        }

        public Entity GetOrDefault(int id)
        {
            return _context.GetConnection().QuerySingleOrDefault<Entity>(
                @"
select * from Entity where Id = @id
", new { id });
        }
    }
}
