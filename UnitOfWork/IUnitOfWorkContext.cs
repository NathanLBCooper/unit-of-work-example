namespace UnitOfWork
{
    public interface IUnitOfWorkContext
    {
        UnitOfWork Create();
    }
}