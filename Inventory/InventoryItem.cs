using System;
using Inventory.Messaging;

namespace Inventory
{
    public class InventoryItem : AggregateRoot
    {
        private string _name;
        private bool _activated;
        private Guid _id;
        private int _count;

        private void Apply(InventoryItemCreated e)
        {
            _id = e.Id;
            _name = e.Name;
            _activated = true;
            _count = 0;
        }

        private void Apply(InventoryItemRenamed e)
        {
            _name = e.NewName;
        }

        private void Apply(InventoryItemDeactivated e)
        {
            _activated = false;
        }

        private void Apply(ItemsCheckedInToInventory e)
        {
            _count += e.Count;
        }

        private void Apply(ItemsRemovedFromInventory e)
        {
            _count -= e.Count;
        }


        public void ChangeName(string newName)
        {
            if (!_activated) throw new InvalidOperationException("item deactivated");
            if (string.IsNullOrEmpty(newName)) throw new ArgumentException("newName");
            ApplyChange(new InventoryItemRenamed(_id, newName));
        }

        public void Remove(int count)
        {
            if (!_activated) throw new InvalidOperationException("item deactivated");
            if (count <= 0) throw new InvalidOperationException("cant remove negative count from inventory");
            if (count > _count) throw new InvalidOperationException(string.Format("cant remove {0} items(s) from inventory (only {1} is available)", count, _count));
            ApplyChange(new ItemsRemovedFromInventory(_id, count));
        }


        public void CheckIn(int count)
        {
            if (!_activated) throw new InvalidOperationException("item deactivated");
            if (count <= 0) throw new InvalidOperationException("must have a count greater than 0 to add to inventory");
            ApplyChange(new ItemsCheckedInToInventory(_id, count));
        }

        public void Deactivate()
        {
            if (!_activated) throw new InvalidOperationException("already deactivated");
            ApplyChange(new InventoryItemDeactivated(_id));
        }

        public override Guid Id
        {
            get { return _id; }
        }

        public InventoryItem() { }

        public InventoryItem(Guid id, string name)
        {
            ApplyChange(new InventoryItemCreated(id, name));
        }
    }
}
