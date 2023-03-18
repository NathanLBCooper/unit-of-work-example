namespace Uow.EntityFramework.Example.Storage;

public interface IGetDbContext
{
    ExampleDbContext GetDbContext();
}
