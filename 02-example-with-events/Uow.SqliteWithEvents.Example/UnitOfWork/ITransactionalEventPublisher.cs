namespace Uow.SqliteWithEvents.Example.UnitOfWork;

public interface ITransactionalEventPublisher : IEventPublisher
{
    void Commit();
}
