using Uow.EntityFrameworkInMemory.Example.Application;

namespace Uow.EntityFrameworkInMemory.Example.Storage;

public class EntityRepository : IEntityRepository
{
    private readonly ExampleDbContext _dbContext;

    public EntityRepository(ExampleDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public int Create(int value)
    {
        var entity = new Entity { Value = value };

        _ = _dbContext.Add(entity);
        _ = _dbContext.SaveChanges();

        return entity.Id!.Value;
    }

    public Entity? GetOrDefault(int id)
    {
        if (_dbContext.Entities.Find(id) is Entity entity)
        {
            return entity;
        }

        return null;
    }
}
