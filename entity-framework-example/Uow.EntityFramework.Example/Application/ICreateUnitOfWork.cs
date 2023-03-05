namespace Uow.EntityFramework.Example.Application;

public interface ICreateUnitOfWork
{
    IUnitOfWork Create();
}
