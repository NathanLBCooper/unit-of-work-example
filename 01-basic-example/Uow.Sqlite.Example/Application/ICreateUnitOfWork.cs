namespace Uow.Sqlite.Example.Application;

public interface ICreateUnitOfWork
{
    IUnitOfWork Create();
}
