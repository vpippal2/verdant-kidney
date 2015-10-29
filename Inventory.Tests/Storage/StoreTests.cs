using System;
using System.Collections.Generic;
using System.Linq;

using Biggy.Core;
using Biggy.Data.Json;

using Inventory.Messaging;
using Inventory.Persistence.Engine;
using Inventory.Persistence.Exceptions;
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
    private ISerializer _serializer;
    [SetUp]
    public void Setup()
    {
      _db = new JsonStore<EventDescriptor>();
      _db.DeleteAll();    
      _serializer= new JsonSerializer();
      _sut = new Store( _db, _serializer);
    }

    [Test]
    public void Store_can_add_new_occurrencie()
    {
      Assert.DoesNotThrow(() => _sut.SaveEvents(_id, GetDummyEvents(1), -1));
    }


    [Test]
    public void Store_can_add_many_new_occurrencies()
    { 
      Assert.DoesNotThrow(() => _sut.SaveEvents(_id, GetDummyEvents(1000), -1));
    }    

    [Test]
    public void Store_can_return_eventList()
    {
      var sent= GetDummyEvents(678000);
      _sut.SaveEvents(_id, sent, -1);

      var retrieved = _sut.GetEventsForAggregate(_id);

      Assert.AreEqual(sent.Count(), retrieved.Count());
      var expected= sent.Max(e=>e.Version);
      var actual= retrieved.Max(e=>e.Version);
      Assert.AreEqual(expected,actual );
    }

    [Test]
    public void Concurrency_exception_must_be_raised()
    {
      var sent = GetDummyEvents(100000);
      _sut.SaveEvents(_id, sent, -1);

      var newDump = GetDummyEvents(100000);      

      Assert.Throws<Concurrency>(() => _sut.SaveEvents(_id, newDump, 678));
    }


    [Test]
    public void events_can_be_deserialized()
    {
      var sent = GetDummyEvents(100000);
      _sut.SaveEvents(_id, sent, -1);

      var list= _sut.GetEventsForAggregate(_id);

      foreach (var item in list)
      {
        Assert.IsInstanceOf<TestEvent>(item);
      }
      
    }

    private IEnumerable<Event> GetDummyEvents(int quantity)
    {
      for (int i = 0; i < quantity; i++)
        yield return new TestEvent("this is " + i +" a demo", i);      
    }
  }
}
