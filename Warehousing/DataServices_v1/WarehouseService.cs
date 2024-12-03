using System.Collections.Generic;
using System.IO;
using System.Text.Json;

namespace Warehousing.DataServices_v1
{
    public class WarehouseService
    {
        private readonly string _filePath = Path.Combine(Directory.GetCurrentDirectory(), "Datasources", "warehouses.json");

        public List<JsonElement> ReadWarehousesFromJson()
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

            var warehouses = JsonSerializer.Deserialize<List<JsonElement>>(jsonData);
            return warehouses ?? new List<JsonElement>();
        }

        public void WriteWarehousesToJson(List<JsonElement> warehouses)
        {
            var jsonData = JsonSerializer.Serialize(warehouses, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(_filePath, jsonData);
        }
    }
}