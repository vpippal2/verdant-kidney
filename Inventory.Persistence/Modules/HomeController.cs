using System;
using System.Collections.Generic;
using Inventory.Messaging;
using Inventory.Persistence.Models;
using Nancy;
using Nancy.OData;

namespace Inventory.Persistence.Modules
{
  public class HomeController:NancyModule
  {
    public HomeController(IStore store):base("/")
    {     

      Get["/{aggregate:guid}"] = _ => 
      {
        var id= Guid.Parse(_.aggregate);
        return  store.GetEventsForAggregate(id);        
      };

      Post["/{aggregate:guid}"] = _ => 200;
    }

    private IEnumerable<Event> GetDummyEvents(int quantity)
    {
      for (int i = 0; i < quantity; i++)
        yield return new TestEvent("this is " + i + " a demo", i);
    }
  }

  public class TestEvent:Event
  {
    private string Data;    

    public TestEvent(string p, int i)
    {
      Data = p;
      Version = i;
    }
 
  }
}