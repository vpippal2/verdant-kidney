using Inventory.Messaging;
using Newtonsoft.Json;

namespace Inventory.Persistence.Engine
{
  public interface ISerializer
  {
    string Serialize(Event @event);  

    Event Deserialize(string data);
  }

}
