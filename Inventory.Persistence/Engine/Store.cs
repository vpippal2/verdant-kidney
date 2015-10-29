using System;
using System.Collections.Generic;
using System.Linq;
using Biggy.Core;
using Inventory.Messaging;
using Inventory.Persistence.Exceptions;
using Inventory.Persistence.Models;

namespace Inventory.Persistence.Engine
{
  public class Store : IStore
  {    
    private readonly IDataStore<EventDescriptor> _db;

    public Store( IDataStore<EventDescriptor> db)
    {
      _db = db;            
    }

    public void SaveEvents(Guid aggregateId, IEnumerable<Event> events, int expectedVersion)
    {      
      var currentVersion=0;
      var eventDescriptors = GetEventsForAggregate(aggregateId);

      if(eventDescriptors.Count>0) currentVersion= eventDescriptors.Max().Version;

      if(currentVersion != expectedVersion && expectedVersion != -1) throw new ConcurrencyException();      
      
      var i = expectedVersion;
      
      foreach (var @event in events)
      {
        i++;
        @event.Version = i;
        _db.Add(new EventDescriptor(aggregateId, @event, i));           
      }     
    }     

    public List<Event> GetEventsForAggregate(Guid aggregateId)
    {      
      return _db.TryLoadData().Where(e => e.Id == aggregateId).Select(e => e.EventData).ToList();      
    }
    
  }
}