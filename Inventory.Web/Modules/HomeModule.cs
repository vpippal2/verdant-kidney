using System;
using Inventory.Messaging;
using Inventory.Web.Services;
using Nancy;

namespace Inventory.Web.Modules
{
  public class HomeModule:NancyModule
  {
    private readonly MiniVan _bus = ServiceLocator.Bus;

    public HomeModule()
    {
      Get["/"]= _=> View["index"];

      Post["/"] = _ =>
      {
        _bus.Send(new CreateInventoryItem(Guid.NewGuid(), Request.Form.name));
        return View["index"];
      };

      Put["/{guid:id}/{int:version}"] = _ =>
      {
        _bus.Send(new RenameInventoryItem(_.id, Request.Form.name, _.version));
        return View["index"];
      };

      Delete["/{guid:id}/{int:version}"] = _ =>
      {
        _bus.Send(new DeactivateInventoryItem(_.id, _.version));
        return View["index"];
      };

      Post["/Checkin/{guid:id}/{int:version}"] = _ =>
      {
        _bus.Send(new CheckInItemsToInventory(_.id, Request.Form.number, _.version));
        return View["index"];
      };


      Post["/Checkout/{guid:id}/{int:version}"] = _ =>
      {
        _bus.Send(new RemoveItemsFromInventory(_.id, Request.Form.number, _.version));
        return View["index"];
      };
    }
  }
}