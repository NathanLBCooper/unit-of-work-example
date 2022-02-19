namespace Uow.SqliteWithEvents.Database.UnitOfWork
{
    public interface ICreateUnitOfWork
    {
        IUnitOfWork Create();
    }
}