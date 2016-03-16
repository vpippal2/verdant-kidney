using System;
using Inventory.Messaging;
using Inventory.Web.Services;
using Nancy;

namespace Inventory.Web.Modules
{
    /// <summary>
    /// Data to fill in HTML form
    /// </summary>
    public class InventoryWebModelData
    {
        public Guid Id { get; set; }
        public int Version { get; set; }
    }


    public class HomeModule : NancyModule
    {

        private readonly MiniVan _bus = ServiceLocator.Bus;

        public HomeModule()
        {
            Get["/"] = _ => View["index", new InventoryWebModelData{Id = new Guid(), Version = -1}];

            Post["/"] = _ =>
            {
                Guid guid = Guid.NewGuid();
                _bus.Send(new CreateInventoryItem(guid, Request.Form.name));
                var model = new InventoryWebModelData {Id = guid, Version = 0};
                return View["index", model];
            };

            Put["/{id:guid}/{version:int}"] = _ =>
            {
                Guid guid = _.id;
                int version = _.version;
                _bus.Send(new RenameInventoryItem(_.id, Request.Form.name, _.version));
                var model = new InventoryWebModelData {Id = guid, Version = version + 1};
                return View["index", model];
            };

            Delete["/{id:guid}/{version:int}"] = _ =>
            {
                _bus.Send(new DeactivateInventoryItem(_.id, _.version));
                return View["index", new InventoryWebModelData()];
            };


            Post["/Checkin/{id:guid}/{version:int}"] = _ =>
            {
                Guid guid = _.id;
                int version = _.version;
                _bus.Send(new CheckInItemsToInventory(guid, Request.Form.number, version));
                var model = new InventoryWebModelData() {Id = guid, Version = version + 1};
                return View["index", model];
            };


            Post["/Checkout/{id:guid}/{version:int}"] = _ =>
            {
                Guid guid = _.id;
                int version = _.version;
                _bus.Send(new RemoveItemsFromInventory(guid, Request.Form.number, version));
                var model = new InventoryWebModelData() { Id = guid, Version = version + 1 };
                return View["index", model];
            };
        }
    }
}