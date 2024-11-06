using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;

public class InventoryService
{
    private readonly string _filePath;

    public InventoryService()
    {
        _filePath = Path.Combine(Directory.GetCurrentDirectory(), "Datasources", "inventories.json");;
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
}