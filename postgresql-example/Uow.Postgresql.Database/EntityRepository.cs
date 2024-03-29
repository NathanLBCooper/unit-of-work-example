﻿using System.Threading.Tasks;
using Dapper;
using Uow.Postgresql.Database.UnitOfWork;

namespace Uow.Postgresql.Database;

public class EntityRepository
{
    private readonly IGetConnection _getConnection;

    public EntityRepository(IGetConnection getConnection)
    {
        _getConnection = getConnection;
    }

    public async Task<int> Create(int value)
    {
        var connection = _getConnection.GetConnection();

        var query = @"
insert into Entity (Value) values (@value) returning Id;
";

        return await connection.QuerySingleAsync<int>(query, new { value });
    }

    public async Task<Entity> GetOrDefault(int id)
    {
        var connection = _getConnection.GetConnection();

        var query = @"
select * from Entity where Id = @id;
";

        return await connection.QuerySingleOrDefaultAsync<Entity>(query, new { id });
    }
}
