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
				var events = store.GetEventsForAggregate (Guid.Parse (_.aggregate)).ToList<Event>();
				return  Response.AsOData(events);        
			};

			Post ["/{aggregate:guid}"] = _ => 200;
		}
	}
}