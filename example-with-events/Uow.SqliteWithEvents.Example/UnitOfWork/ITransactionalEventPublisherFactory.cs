namespace Uow.SqliteWithEvents.Example.UnitOfWork;

public interface ITransactionalEventPublisherFactory
{
    ITransactionalEventPublisher Create();
}
