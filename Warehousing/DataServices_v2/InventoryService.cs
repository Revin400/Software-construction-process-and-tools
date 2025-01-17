using System;
using System.Collections.Generic;
using System.Linq;


namespace Warehousing.DataServices_v2
{
    public class InventoryService
    {

        public WarehousingContext _context;

        public InventoryService(WarehousingContext context)
        {
            _context = context;
            _context.Database.EnsureCreated();
        }

        public List<Inventory> GetAllInventories()
        {
            return _context.Inventories.ToList();
        }

        public Inventory GetInventoryById(int id)
        {
            return _context.Inventories.FirstOrDefault(x => x.Id == id);
        }

        public List<Inventory> GetInventoryByItemId(string itemId)
        {
            return _context.Inventories.Where(x => x.ItemId == itemId).ToList();
        }

        public Dictionary<string, int> GetInventoryTotalsForItem(string itemId)
        {
            var result = new Dictionary<string, int>
            {
                { "total_expected", 0 },
                { "total_ordered", 0 },
                { "total_allocated", 0 },
                { "total_available", 0 }
            };

            var inventories = _context.Inventories.Where(x => x.ItemId == itemId).ToList();

            foreach (var inventory in inventories)
            {
                result["total_expected"] += inventory.TotalExpected;
                result["total_ordered"] += inventory.TotalOrdered;
                result["total_allocated"] += inventory.TotalAllocated;
                result["total_available"] += inventory.TotalAvailable;
            }

            return result;
        }

        public void AddInventory(Inventory inventory)
        {
            inventory.CreatedAt = DateTime.Now;
            inventory.UpdatedAt = DateTime.Now;
            _context.Inventories.Add(inventory);
            _context.SaveChanges();
        }

        public void UpdateInventory(Inventory inventory)
        {
            var existingInventory = _context.Inventories.FirstOrDefault(x => x.Id == inventory.Id);
            if (existingInventory == null) return;

            inventory.CreatedAt = existingInventory.CreatedAt;
            inventory.UpdatedAt = DateTime.Now;
            _context.Entry(existingInventory).CurrentValues.SetValues(inventory);
            _context.SaveChanges();
        }

        public void DeleteInventory(int id)
        {
            var inventory = _context.Inventories.FirstOrDefault(x => x.Id == id);
            if (inventory != null)
            {
                _context.Inventories.Remove(inventory);
                _context.SaveChanges();
            }
        }

    }
}