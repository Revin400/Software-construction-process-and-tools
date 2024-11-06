
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using System;

[Route("api/warehouse")]
[ApiController]
public class Inventories : ControllerBase
{
    private readonly string dataPath;
    private List<Inventory> data;

    public Inventories(string rootPath, bool isDebug = false)
    {
        dataPath = Path.Combine(rootPath, "inventories.json");
        Load(isDebug);
    }

    public List<Inventory> GetInventories()
    {
        return data;
    }

    public Inventory GetInventory(int inventoryId)
    {
        return data.Find(x => x.Id == inventoryId);
    }

    public List<Inventory> GetInventoriesForItem(int itemId)
    {
        return data.FindAll(x => x.ItemId == itemId);
    }

    public Dictionary<string, int> GetInventoryTotalsForItem(int itemId)
    {
        var result = new Dictionary<string, int>
        {
            { "total_expected", 0 },
            { "total_ordered", 0 },
            { "total_allocated", 0 },
            { "total_available", 0 }
        };

        foreach (var item in data)
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
        inventory.CreatedAt = GetTimestamp();
        inventory.UpdatedAt = GetTimestamp();
        data.Add(inventory);
    }

    public void UpdateInventory(int inventoryId, Inventory inventory)
    {
        inventory.UpdatedAt = GetTimestamp();
        var index = data.FindIndex(x => x.Id == inventoryId);
        if (index >= 0)
        {
            data[index] = inventory;
        }
    }

    public void RemoveInventory(int inventoryId)
    {
        data.RemoveAll(x => x.Id == inventoryId);
    }

    private void Load(bool isDebug)
    {
        if (isDebug)
        {
            data = new List<Inventory>
            {
                new Inventory
                {
                    Id = 1,
                    ItemId = 1,
                    Description = "High Precision Bearings",
                    ItemReference = "REF123",
                    LocationId = 1,
                    TotalOnHand = 150,
                    TotalExpected = 200,
                    TotalOrdered = 50,
                    TotalAllocated = 100,
                    TotalAvailable = 50,
                    CreatedAt = DateTime.Parse("2024-03-20T10:00:00Z"),
                    UpdatedAt = DateTime.Parse("2024-03-21T11:00:00Z")
                },
                new Inventory
                {
                    Id = 2,
                    ItemId = 1,
                    Description = "High Precision Bearings",
                    ItemReference = "REF123",
                    LocationId = 2,
                    TotalOnHand = 150,
                    TotalExpected = 200,
                    TotalOrdered = 50,
                    TotalAllocated = 100,
                    TotalAvailable = 50,
                    CreatedAt = DateTime.Parse("2024-03-20T10:00:00Z"),
                    UpdatedAt = DateTime.Parse("2024-03-21T11:00:00Z")
                }
                // Additional inventory entries can be added here
            };
        }
        else
        {
            data = JsonSerializer.Deserialize<List<Inventory>>(System.IO.File.ReadAllText(dataPath));
        }
    }

    public void Save()
    {
        System.IO.File.WriteAllText(dataPath, JsonSerializer.Serialize(data));
    }

    private DateTime GetTimestamp()
    {
        return DateTime.UtcNow;
    }
}
