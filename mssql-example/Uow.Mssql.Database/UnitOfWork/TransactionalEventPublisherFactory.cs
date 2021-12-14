namespace Uow.Mssql.Database.UnitOfWork
{
    public class TransactionalEventPublisherFactory : ITransactionalEventPublisherFactory
    {
        public ITransactionalEventPublisher Create() => new TransactionalEventPublisher();
    }
}
