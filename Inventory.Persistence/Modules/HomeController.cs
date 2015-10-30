using System;
using System.Collections.Generic;

using Inventory.Messaging;

using Nancy;
using Nancy.OData;

namespace Inventory.Persistence.Modules
{
  public class HomeController:NancyModule
  {
    public HomeController (IStore store) : base ("/")
		{     

			Get ["/{aggregate:guid}"] = _ => {
				IList<Event> events = store.GetEventsForAggregate (Guid.Parse (_.aggregate));
				return  Response.AsOData(events);        
			};

			Post ["/{aggregate:guid}"] = _ => 200;
		}
	}
}