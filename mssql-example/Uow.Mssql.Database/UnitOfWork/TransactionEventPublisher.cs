using System.Collections.Generic;

namespace Uow.Mssql.Database.UnitOfWork
{
    public class TransactionalEventPublisher : ITransactionalEventPublisher
    {
        private readonly List<EntityEvent> _events;

        public TransactionalEventPublisher()
        {
            _events = new List<EntityEvent>();
        }

        public void PublishEvent(EntityEvent @event)
        {
            _events.Add(@event);
        }

        public void Commit()
        {
            // Publish the events in some way
        }
    }
}
