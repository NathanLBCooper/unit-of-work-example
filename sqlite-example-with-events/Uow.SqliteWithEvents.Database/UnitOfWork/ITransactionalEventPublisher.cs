namespace Uow.SqliteWithEvents.Database.UnitOfWork
{
    public interface ITransactionalEventPublisher : IEventPublisher
    {
        void Commit();
    }
}
