using System;
using System.Collections.Generic;
using Inventory.Messaging;
using Inventory.Persistence.Models;
namespace Inventory.Tests.Domain
{
  public class FakeAggregate:AggregateRoot
  {
    private Guid _id;
    // really don't needed for bussines process only to test repo
    public string Name;

    public FakeAggregate(){}

    public override Guid Id
    {
      get { return _id ;}
    }    

    public FakeAggregate(Guid id, string name)
    {
      ApplyChange(new FakeCreated(id, name));
    }

    public void Apply(FakeCreated e)
    {
      _id = e.Id;
      Name = e.Name;
    }
  }

  public class FakeCreated:Event
  {
    public readonly Guid Id;
    public readonly string Name;

    public FakeCreated(Guid id, string name)
    {
      Id = id;
      Name = name;
    }
  }
}
