using System;
using Inventory.Messaging;
using Inventory.Web.Services;
using Nancy;

namespace Inventory.Web.Modules
{
/*    public class InventoryWebModelData
    {
        public bool Defined = false;
        public Guid Id { get; set; }
        public int Version { get; set; }
    }
*/

    public class HomeModule : NancyModule
    {


        private readonly MiniVan _bus = ServiceLocator.Bus;

        public HomeModule()
        {
            Get["/"] = _ => View["index"];

            Post["/"] = _ =>
            {
                Guid guid = Guid.NewGuid();
                _bus.Send(new CreateInventoryItem(guid, Request.Form.name));
                return View["index"];
            };

            Put["/{id:guid}/{version:int}"] = _ =>
            {
                _bus.Send(new RenameInventoryItem(_.id, Request.Form.name, _.version));
                return View["index"];
            };

            Delete["/{id:guid}/{version:int}"] = _ =>
            {
                _bus.Send(new DeactivateInventoryItem(_.id, _.version));
                return View["index"];
            };


            Post["/Checkin/{id:guid}/{version:int}"] = _ =>
            {
                _bus.Send(new CheckInItemsToInventory(_.id, Request.Form.number, _.version));
                return View["index"];
            };


            Post["/Checkout/{id:guid}/{version:int}"] = _ =>
            {
                _bus.Send(new RemoveItemsFromInventory(_.id, Request.Form.number, _.version));
                return View["index"];
            };
        }
    }
}