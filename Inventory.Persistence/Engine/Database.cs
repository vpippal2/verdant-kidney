using Biggy.Core;
using Biggy.Data.Json;

namespace Inventory.Persistence.Engine
{
  public class Database : PersistenceEngine
  {
    public readonly JsonDbCore DB;

    public Database(bool dropCreateTables = false)
    {
      DB = new JsonDbCore();
      if (dropCreateTables) this.DropCreateAll();

      this.LoadData();
    }


    public Database(string name, bool dropCreateTables = false)
    {
      DB = new JsonDbCore(name);
      if (dropCreateTables) this.DropCreateAll();

      this.LoadData();
    }   

    public override IDataStore<T> CreateDocumentStoreFor<T>()
    {
      return DB.CreateStoreFor<T>();
    }

    public override void DropCreateAll()
    {
       DB.TryDropTable("Events");      
    }

  }
   
}