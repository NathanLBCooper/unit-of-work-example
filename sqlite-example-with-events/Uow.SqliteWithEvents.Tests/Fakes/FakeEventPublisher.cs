using System.Collections.Generic;
using Uow.SqliteWithEvents.Database;
using Uow.SqliteWithEvents.Database.UnitOfWork;

namespace Uow.SqliteWithEvents.Tests.Fakes
{
    public class FakeEventPublisher : ITransactionalEventPublisher
    {
        public readonly List<EntityEvent> Staged = new();
        public readonly List<EntityEvent> Committed = new();

        public void PublishEvent(EntityEvent @event)
        {
            Staged.Add(@event);
        }

        public void Commit()
        {
            Committed.AddRange(Staged);
            Staged.Clear();
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
