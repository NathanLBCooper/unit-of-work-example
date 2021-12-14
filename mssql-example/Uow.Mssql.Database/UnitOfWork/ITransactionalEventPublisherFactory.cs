namespace Uow.Mssql.Database.UnitOfWork
{
    public interface ITransactionalEventPublisherFactory
    {
        ITransactionalEventPublisher Create();
    }
}
