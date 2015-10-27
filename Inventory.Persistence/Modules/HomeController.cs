using Nancy;

namespace Inventory.Persistence.Modules
{
  public class HomeController:NancyModule
  {
    public HomeController():base("/")
    {
      Get["/"] = _ => "hi there";
    }
  }
}