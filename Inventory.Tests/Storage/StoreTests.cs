using System;
using System.Collections.Generic;
using System.Linq;

using Biggy.Core;
using Biggy.Data.Json;

using Inventory.Messaging;
using Inventory.Persistence.Engine;
using Inventory.Persistence.Models;

using NUnit.Framework;

namespace Inventory.Tests.Storage
{
  [TestFixture]
  public class StoreTests
  {
    
    private IDataStore<EventDescriptor> _db;
    private Guid _id = Guid.NewGuid();
    private IStore _sut;

    [SetUp]
    public void Setup()
    {
      _db = new JsonStore<EventDescriptor>();
      _db.DeleteAll();    
      _sut = new Store( _db);
    }

    [Test]
    public void Store_can_add_new_occurrencie()
    {
      Assert.DoesNotThrow(() => _sut.SaveEvents(_id, GetDummyEvents(1), -1));
    }


    [Test]
    public void Store_can_add_new_occurrencies()
    { 
      Assert.DoesNotThrow(() => _sut.SaveEvents(_id, GetDummyEvents(100), -1));
    }    

    [Test]
    public void Store_can_return_eventList()
    {
      var sent= GetDummyEvents(678);
      _sut.SaveEvents(_id, sent, -1);

      var retrieved = _sut.GetEventsForAggregate(_id);

      Assert.AreEqual(sent.Count(), retrieved.Count());
      var expected= sent.Max(e=>e.Version);
      var actual= retrieved.Max(e=>e.Version);
      Assert.AreEqual(expected,actual );

    }


    private IEnumerable<Event> GetDummyEvents(int quantity)
    {
      for (int i = 0; i < quantity; i++)
      {
        yield return new TestEvent("this is a demo", i);
      }
    }
  }
}
