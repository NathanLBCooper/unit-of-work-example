namespace Uow.EntityFrameworkInMemory.Example.Application;

public interface ICreateUnitOfWork
{
    IUnitOfWork Create();
}
