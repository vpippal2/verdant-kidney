
namespace Inventory.Messaging
{
  public interface Message{}

  public class Event : Message
  {
    public int Version;
  }

  public class Command : Message{}
}
