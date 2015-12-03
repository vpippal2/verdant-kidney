using System;
using System.Collections.Generic;

namespace Inventory.Web.Services
{
  public class RestStore:IStore
  {
    public void SaveEvents(Guid aggregateId, IEnumerable<Messaging.Event> events, int expectedVersion)
    {
      throw new NotImplementedException();
    }

    public List<Messaging.Event> GetEventsForAggregate(Guid aggregateId)
    {
      throw new NotImplementedException();
    }
  }
}