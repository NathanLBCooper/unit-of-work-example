namespace Uow.EntityFramework.Example.Application;

public interface IEntityRepository
{
    int Create(int value);
    Entity? GetOrDefault(int id);
}
