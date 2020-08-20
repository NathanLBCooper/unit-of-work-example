namespace UnitOfWork.SQLite
{
    public interface IUnitOfWorkContext
    {
        IUnitOfWork Create();
    }
}
