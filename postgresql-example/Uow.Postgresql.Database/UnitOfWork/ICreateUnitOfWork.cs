namespace Uow.Postgresql.Database.UnitOfWork;

public interface ICreateUnitOfWork
{
    IUnitOfWork Create();
}
