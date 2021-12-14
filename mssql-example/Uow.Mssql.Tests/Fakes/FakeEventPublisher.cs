using System.Collections.Generic;
using Uow.Mssql.Database;
using Uow.Mssql.Database.UnitOfWork;

namespace Uow.Mssql.Tests.Fakes
{
    public class FakeEventPublisher : ITransactionalEventPublisher
    {
        public readonly List<EntityEvent> StagedEntityEvents = new();
        public readonly List<EntityEvent> CommittedEnityEvents = new();

        public void PublishEvent(EntityEvent @event)
        {
            StagedEntityEvents.Add(@event);
        }

        public void Commit()
        {
            CommittedEnityEvents.AddRange(StagedEntityEvents);
            StagedEntityEvents.Clear();
        }
    }

    public class FakeTransactionalEventPublisherFactory : ITransactionalEventPublisherFactory
    {
        public FakeEventPublisher CurrentEventPublisher;

        public ITransactionalEventPublisher Create()
        {
            CurrentEventPublisher = new FakeEventPublisher();
            return CurrentEventPublisher;
        }
    }
}
