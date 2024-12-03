using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;



namespace Warehousing.DataServices_v1
{
    public class ItemService
    {
        private const string FilePath = "items.json";
        private List<Item> _items;

        public ItemService()
        {
            _items = LoadItems();
        }
        private List<Item> LoadItems()
        {
            if (!File.Exists(FilePath))
            {
                return new List<Item>();
            }

            var json = File.ReadAllText(FilePath);
            return JsonSerializer.Deserialize<List<Item>>(json) ?? new List<Item>();
        }


        private void SaveItems()
        {
            var json = JsonSerializer.Serialize(_items, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(FilePath, json);
        }
        
        public IEnumerable<Item> GetAllItems() => _items.ToList();

        public Item GetItemById(int id)
        {
            var item = _items.FirstOrDefault(x => x.Id == id);
            if (item == null) return null;
            return item;
        }

        public IEnumerable<Item> GetItemsByItemLine(int itemLineId)
        {

        var x = _items.Where(x => x.ItemLineId == itemLineId).ToList();
        if (x.Count == 0) return [];
        return x;
        }
        
        public IEnumerable<Item> GetItemsByItemGroup(int itemGroupId)
        {
        
        var x = _items.Where(x => x.ItemGroupId == itemGroupId).ToList();
        if (x.Count == 0) return [];
        return x;
        }

        public IEnumerable<Item> GetItemsByItemType(int itemTypeId)
        {
            var x = _items.Where(x => x.ItemTypeId == itemTypeId).ToList();
            if (x.Count == 0) return [];
            return x;
        }
        public IEnumerable<Item> GetItemsBySupplier(int supplierId)
        {
            var x = _items.Where(x => x.SupplierId == supplierId).ToList();
            if (x.Count == 0) return [];
            return x;
        }
        public void AddItem(Item item)
        {
            item.CreatedAt = item.UpdatedAt = DateTime.UtcNow;
            _items.Add(item);
            SaveItems();
        }

        public void UpdateItem(int id, Item updatedItem)
        {
            updatedItem.UpdatedAt = DateTime.UtcNow;
            var existingItem = _items.FirstOrDefault(x => x.Id == id);
            if (existingItem != null)
            {
                SaveItems();
                
            }
            
        }

        public void DeleteItem(int id)
        {
            var item = _items.FirstOrDefault(x => x.Id == id);
            _items.Remove(item);
            SaveItems();
        }

        
    }

    
}
