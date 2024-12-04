using System.Collections.Generic;
using System.IO;
using System.Text.Json;

namespace Warehousing.DataServices_v1
{
    public class ShipmentService
    {
        private readonly string _filePath = Path.Combine(Directory.GetCurrentDirectory(), "Datasources", "Shipment.json");

        public List<JsonElement> ReadShipmentsFromJson()
        {
            if (!File.Exists(_filePath))
            {
                File.WriteAllText(_filePath, "[]");
                return new List<JsonElement>();
            }

            var jsonData = File.ReadAllText(_filePath);
            if (string.IsNullOrWhiteSpace(jsonData))
            {
                return new List<JsonElement>();
            }

            var shipments = JsonSerializer.Deserialize<List<JsonElement>>(jsonData);
            return shipments ?? new List<JsonElement>();
        }

        public void WriteShipmentsToJson(List<JsonElement> shipments)
        {
            var jsonData = JsonSerializer.Serialize(shipments, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(_filePath, jsonData);
        }
    }
}