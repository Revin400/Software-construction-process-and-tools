using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;

public class InventoryService
{
    private readonly string _filePath;

    public InventoryService(string filePath)
    {
        _filePath = filePath;
    }

    public List<Inventory> ReadInventoriesFromJson()
    {
        if (!File.Exists(_filePath))
        {
            return new List<Inventory>();
        }

        var jsonData = File.ReadAllText(_filePath);

        if (string.IsNullOrWhiteSpace(jsonData))
        {
            return new List<Inventory>();
        }
        return JsonSerializer.Deserialize<List<Inventory>>(jsonData) ?? new List<Inventory>();
    }

    public void WriteInventoriesToJson(List<Inventory> inventories)
    {
        var jsonData = JsonSerializer.Serialize(inventories, new JsonSerializerOptions { WriteIndented = true });
        File.WriteAllText(_filePath, jsonData);
    }

    public int NextId()
    {
        var inventories = ReadInventoriesFromJson();
        return inventories.Any() ? inventories.Max(i => i.Id) + 1 : 1;
    }

    public IEnumerable<Inventory> GetInventories()
    {
        return ReadInventoriesFromJson();
    }

    public Inventory GetInventory(int inventoryId)
    {
        return ReadInventoriesFromJson().FirstOrDefault(i => i.Id == inventoryId);
    }

    public Dictionary<string, decimal> GetInventoriesForItem(int itemId)
    {
        var result = new Dictionary<string, decimal>
        {
            { "total_expected", 0 },
            { "total_ordered", 0 },
            { "total_allocated", 0 },
            { "total_available", 0 }
        };

        var inventories = ReadInventoriesFromJson();
        foreach (var item in inventories)
        {
            if (item.ItemId == itemId)
            {
                result["total_expected"] += item.TotalExpected;
                result["total_ordered"] += item.TotalOrdered;
                result["total_allocated"] += item.TotalAllocated;
                result["total_available"] += item.TotalAvailable;
            }
        }
        return result;
    }

    public void AddInventory(Inventory inventory)
    {
        var inventories = ReadInventoriesFromJson();
        inventory.Id = NextId();
        inventory.CreatedAt = DateTime.UtcNow;
        inventory.UpdatedAt = DateTime.UtcNow;
        inventories.Add(inventory);
        WriteInventoriesToJson(inventories);
    }

    public bool UpdateInventory(int inventoryId, Inventory inventory)
    {
        var inventories = ReadInventoriesFromJson();
        var existingInventory = inventories.FirstOrDefault(i => i.Id == inventoryId);
        if (existingInventory == null)
        {
            return false;
        }
        inventory.UpdatedAt = DateTime.UtcNow;
        inventories[inventories.IndexOf(existingInventory)] = inventory;
        WriteInventoriesToJson(inventories);
        return true;
    }

    public bool RemoveInventory(int inventoryId)
    {
        var inventories = ReadInventoriesFromJson();
        var inventory = inventories.FirstOrDefault(i => i.Id == inventoryId);
        if (inventory == null)
        {
            return false;
        }
        inventories.Remove(inventory);
        WriteInventoriesToJson(inventories);
        return true;
    }
}