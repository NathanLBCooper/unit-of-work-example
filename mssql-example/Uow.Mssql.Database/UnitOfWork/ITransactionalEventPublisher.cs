namespace Uow.Mssql.Database.UnitOfWork
{
    public interface ITransactionalEventPublisher : IEventPublisher
    {
        void Commit();
    }
}
