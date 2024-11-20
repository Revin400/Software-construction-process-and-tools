using System.Collections.Generic;
using System.IO;
using System.Text.Json;

public class ShipmentService
{
    private readonly string _filePath = Path.Combine(Directory.GetCurrentDirectory(), "Datasources", "Shipment.json");

    public List<Shipment> ReadshipmentsFromJson()
    {
        if (!File.Exists(_filePath))
        {
            File.WriteAllText(_filePath, "[]");  
            return new List<Shipment>(); 
        }

        var jsonData = File.ReadAllText(_filePath);

        if (string.IsNullOrWhiteSpace(jsonData))
        {
            return new List<Shipment>();
        }
        return JsonSerializer.Deserialize<List<Shipment>>(jsonData) ?? new List<Shipment>();
    }

    public void WriteShipmentsToJson(List<Shipment> shipments)
    {
        var jsonData = JsonSerializer.Serialize(shipments, new JsonSerializerOptions { WriteIndented = true });
        File.WriteAllText(_filePath, jsonData);
    }

    public int NextId()
    {
        var shipments = ReadshipmentsFromJson();
        return shipments.Any() ? shipments.Max(w => w.Id) + 1 : 1; 
    }
    
}