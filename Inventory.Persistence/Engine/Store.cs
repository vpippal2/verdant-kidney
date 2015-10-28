using System;
using System.Collections.Generic;
using Inventory.Messaging;
using System.Linq;

namespace Inventory.Persistence.Engine
{
  public class Store : IStore
  {
    private readonly IEventPublisher _publisher;
    private readonly Database _db;

    public Store(IEventPublisher publisher)
    {
      _db = new Database();
      _publisher = publisher;
    }

    public void SaveEvents(Guid aggregateId, IEnumerable<Messaging.Event> events, int expectedVersion)
    {
      
    }

    public List<Event> GetEventsForAggregate(Guid aggregateId)
    {
      return (from e in _db.Events where e.Id == aggregateId select e.EventData).ToList();      
    }
  }
}