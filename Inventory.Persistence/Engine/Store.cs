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
    private readonly ISerializer _serializer;

    public Store( IDataStore<EventDescriptor> db, ISerializer serializer)
    {
      _serializer = serializer;
      _db = db;            
    }

    public void SaveEvents(Guid aggregateId, IEnumerable<Event> events, int expectedVersion)
    {
      var myDump = new List<EventDescriptor>();

      var currentVersion = _db.TryLoadData().Exists(e=>e.Id== aggregateId) ?
              _db.TryLoadData().Where(e => e.Id == aggregateId).Max(e=>e.Version)             
             : 0;
      
      if(currentVersion != expectedVersion && expectedVersion != -1) throw new Concurrency();      
      
      var i = expectedVersion;
      
      foreach (var @event in events)
      {
        i++;
        @event.Version = i;
        myDump.Add(new EventDescriptor(aggregateId, _serializer.Serialize(@event), i));           
      }
      _db.Add(myDump);

    }     

    public List<Event> GetEventsForAggregate(Guid aggregateId)
    {
      var eventDescriptors = _db.TryLoadData().Where(e => e.Id == aggregateId).ToList();

      if (eventDescriptors.Count() <= 0) throw new AggregateNotFound();

      var events = new List<Event>();
      foreach (var record in eventDescriptors)
        events.Add(_serializer.Deserialize(record.EventData));

      return events;
    }
    
  }
}