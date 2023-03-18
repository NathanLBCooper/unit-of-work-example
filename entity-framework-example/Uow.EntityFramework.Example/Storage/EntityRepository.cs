using Uow.EntityFramework.Example.Application;

namespace Uow.EntityFramework.Example.Storage;

public class EntityRepository : IEntityRepository
{
    private readonly IGetDbContext _getDbContext;

    public EntityRepository(IGetDbContext getDbContext)
    {
        _getDbContext = getDbContext;
    }

    public int Create(int value)
    {
        var entity = new Entity { Value = value };

        var context = _getDbContext.GetDbContext();

        _ = context.Add(entity);
        _ = context.SaveChanges();

        return entity.Id!.Value;
    }

    public Entity? GetOrDefault(int id)
    {
        var context = _getDbContext.GetDbContext();

        if (context.Entities.Find(id) is Entity entity)
        {
            return entity;
        }

        return null;
    }
}
