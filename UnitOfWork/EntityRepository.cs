using System.Threading.Tasks;
using Dapper;

namespace UnitOfWork
{
    public class EntityRepository
    {
        private readonly IConnectionContext _context;

        public EntityRepository(IConnectionContext context)
        {
            _context = context;
        }

        public async Task<int> CreateAsync(int value)
        {
            return await _context.GetConnection().QuerySingleAsync<int>(
                @"
insert into Entity (Value) values (@value);
select last_insert_rowid();
", new { value });
        }

        public async Task<Entity> GetOrDefaultAsync(int id)
        {
            return await _context.GetConnection().QuerySingleOrDefaultAsync<Entity>(
                @"
select * from Entity where Id = @id
", new { id });
        }
    }
}
