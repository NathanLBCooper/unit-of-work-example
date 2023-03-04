namespace Uow.SqliteWithEvents.Example.UnitOfWork;

public interface IEventPublisher
{
    void PublishEvent(EntityEvent @event);
}
