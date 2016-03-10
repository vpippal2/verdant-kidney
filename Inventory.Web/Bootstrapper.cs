using Inventory.Handlers;
using Inventory.Messaging;
using Inventory.Persistence;
using Inventory.Web.Services;
using Nancy;
using Nancy.Bootstrapper;

namespace Inventory.Web
{
    // Set PersistenceBootstrapper as parrent to allow usage of "Inventory.Persistence" in Web project
    // NOTE: the object is created automaticaly (inherited is created while parent is not)
    public class Bootstrapper: PersistenceBootstrapper //DefaultNancyBootstrapper
    {
      protected override void ApplicationStartup(Nancy.TinyIoc.TinyIoCContainer container, Nancy.Bootstrapper.IPipelines pipelines)
    {
      Wiring.Wire();
      base.ApplicationStartup(container, pipelines);
    }
  }

  public static class Wiring
  {
    public static void Wire()
    {
      var bus = new MiniVan();

      var storage = new RestStore();
      var rep = new Repository<InventoryItem>(storage);
      var commands = new InventoryCommandHandlers(rep);
      bus.RegisterHandler<CheckInItemsToInventory>(commands.Handle);
      bus.RegisterHandler<CreateInventoryItem>(commands.Handle);
      bus.RegisterHandler<DeactivateInventoryItem>(commands.Handle);
      bus.RegisterHandler<RemoveItemsFromInventory>(commands.Handle);
      bus.RegisterHandler<RenameInventoryItem>(commands.Handle);        
      ServiceLocator.Bus = bus;
    }
  }
}