namespace Uow.Mssql.Database.UnitOfWork
{
    public interface IEventPublisher
    {
        void PublishEvent(EntityEvent @event);
    }
}