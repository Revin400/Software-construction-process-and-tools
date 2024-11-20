using System.Collections.Generic;
using System.IO;
using System.Text.Json;

public class WarehouseService
{
    private readonly string _filePath = Path.Combine(Directory.GetCurrentDirectory(), "Datasources", "warehouses.json");

    public List<Warehouse> ReadWarehousesFromJson()
    {
        if (!File.Exists(_filePath))
        {
            File.WriteAllText(_filePath, "[]");  
            return new List<Warehouse>(); 
        }

        var jsonData = File.ReadAllText(_filePath);

        if (string.IsNullOrWhiteSpace(jsonData))
        {
            return new List<Warehouse>();
        }
        return JsonSerializer.Deserialize<List<Warehouse>>(jsonData) ?? new List<Warehouse>();
    }

    public void WriteWarehousesToJson(List<Warehouse> warehouses)
    {
        var jsonData = JsonSerializer.Serialize(warehouses, new JsonSerializerOptions { WriteIndented = true });
        File.WriteAllText(_filePath, jsonData);
    }

    public int NextId()
    {
        var warehouses = ReadWarehousesFromJson();
        return warehouses.Any() ? warehouses.Max(w => w.Id) + 1 : 1; 
    }
}
