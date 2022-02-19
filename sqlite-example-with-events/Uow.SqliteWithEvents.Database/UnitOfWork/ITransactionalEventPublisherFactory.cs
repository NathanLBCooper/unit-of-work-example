namespace Uow.SqliteWithEvents.Database.UnitOfWork
{
    public interface ITransactionalEventPublisherFactory
    {
        ITransactionalEventPublisher Create();
    }
}
