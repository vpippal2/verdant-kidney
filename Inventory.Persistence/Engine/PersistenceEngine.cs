using Biggy.Core;
using Inventory.Persistence.Models;

namespace Inventory.Persistence.Engine
{
  public abstract class PersistenceEngine
  {
    public BiggyList<EventDescriptor> Events { get; set; }    
    
    public virtual void LoadData()
    {
      this.Events = new BiggyList<EventDescriptor>(CreateDocumentStoreFor<EventDescriptor>());     
    }
    
    public abstract void DropCreateAll();    
    public abstract IDataStore<T> CreateDocumentStoreFor<T>() where T : new();  
  }
}