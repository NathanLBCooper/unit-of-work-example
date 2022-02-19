using System.Threading.Tasks;
using Dapper;
using Uow.Mssql.Database.UnitOfWork;

namespace Uow.Mssql.Database
{
    public class EntityRepository
    {
        private readonly IGetUnitOfWork _getUnitOfWork;

        public EntityRepository(IGetUnitOfWork getUnitOfWork)
        {
            _getUnitOfWork = getUnitOfWork;
        }

        public async Task<int> Create(int value)
        {
            var (connection, transaction) = _getUnitOfWork.GetConnection();

            var query = @"
insert into Entity (Value) values (@value);
select SCOPE_IDENTITY();
";

            return await connection.QuerySingleAsync<int>(query, new { value }, transaction);
        }

        public async Task<Entity> GetOrDefault(int id)
        {
            var (connection, transaction) = _getUnitOfWork.GetConnection();

            var query = @"
select * from Entity where Id = @id;
";

            return await connection.QuerySingleOrDefaultAsync<Entity>(query, new { id }, transaction);
        }
    }
}
