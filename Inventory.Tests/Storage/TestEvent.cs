using Inventory.Messaging;

namespace Inventory.Tests.Storage
{
  public class TestEvent:Event
  {
    public readonly string Data;

    public TestEvent(string data,int version)
    {
      Data= data;
      Version = version;
    }
  }
}
