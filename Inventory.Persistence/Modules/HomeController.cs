using Nancy;

namespace Inventory.Persistence.Modules
{
  public class HomeController:NancyModule
  {
    public HomeController():base("/")
    {
      Get["/{aggregate:guid}"] = _ => "hi there";

      Post["/{aggregate:guid}"] = _ => "hi there";
    }
  }
}