namespace Uow.SqliteWithEvents.Database.UnitOfWork
{
    public interface IEventPublisher
    {
        void PublishEvent(EntityEvent @event);
    }
}
