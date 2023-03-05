namespace Uow.EntityFramework.Example.Application;

public class Controller
{
    private readonly IEntityRepository _entityRepository;
    private readonly ICreateUnitOfWork _createUnitOfWork;

    public Controller(IEntityRepository entityRepository, ICreateUnitOfWork createUnitOfWork)
    {
        _entityRepository = entityRepository;
        _createUnitOfWork = createUnitOfWork;
    }

    public Entity? GetOrDefault(int id)
    {
        using var uow = _createUnitOfWork.Create();

        return _entityRepository.GetOrDefault(id);
    }

    public int CreateAndCommit(Entity entity)
    {
        using var uow = _createUnitOfWork.Create();

        var id = _entityRepository.Create(entity.Value);
        uow.Commit();

        return id;
    }

    public int CreateAndRollback(Entity entity)
    {
        using var uow = _createUnitOfWork.Create();

        var id = _entityRepository.Create(entity.Value);
        uow.RollBack();

        return id;
    }

    public int CreateAndNeitherCommitNorRollback(Entity entity)
    {
        using var uow = _createUnitOfWork.Create();

        var id = _entityRepository.Create(entity.Value);

        return id;
    }
}
