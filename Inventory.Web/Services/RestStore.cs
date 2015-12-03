using System;
using System.Collections.Generic;
using Inventory.Messaging;

namespace Inventory.Web.Services
{
  public class RestStore:IStore
  {
    public void SaveEvents(Guid aggregateId, IEnumerable<Event> events, int expectedVersion)
    {
      throw new NotImplementedException();
    }

    public List<Event> GetEventsForAggregate(Guid aggregateId)
    {
      throw new NotImplementedException();
    }
  }
}