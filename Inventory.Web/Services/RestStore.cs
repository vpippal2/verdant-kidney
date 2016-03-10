using System;
using System.Collections.Generic;
using Biggy.Data.Json;
using Inventory.Messaging;
using Inventory.Persistence.Engine;

//using Inventory.Persistence.Engine;
//using Inventory.Persistence.Models;
//using Biggy.Data.Json;

namespace Inventory.Web.Services
{
    public class RestStore : IStore
    {
        private Store _persistentStore;

        public RestStore()
        {
            //TODO: use persistent store ????
            _persistentStore = new Store(new JsonStore<Persistence.Models.EventDescriptor>(@"c:\temp\_a_", "webstore", "events"), new JsonSerializer());
        }

        //private Persistence.Engine.Store _persistentStore;

        public void SaveEvents(Guid aggregateId, IEnumerable<Event> events, int expectedVersion)
        {
            //throw new NotImplementedException();
            //TODO: use persistent store ????
            _persistentStore.SaveEvents(aggregateId, events, expectedVersion);
        }

        public List<Event> GetEventsForAggregate(Guid aggregateId)
        {
            //throw new NotImplementedException();
            //TODO: use persistent store ????
            return _persistentStore.GetEventsForAggregate(aggregateId);
        }
    }
}