namespace Uow.Mssql.Database.UnitOfWork
{
    public interface ICreateUnitOfWork
    {
        IUnitOfWork Create();
    }
}