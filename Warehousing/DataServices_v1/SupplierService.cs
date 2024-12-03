using System.Collections.Generic;
using System.IO;
using System.Text.Json;

namespace Warehousing.DataServices_v1
{
    public class SupplierService
    {
        private readonly string _filePath = Path.Combine(Directory.GetCurrentDirectory(), "Datasources", "suppliers.json");

        public List<JsonElement> ReadsuppliersFromJson()
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

            var suppliers = JsonSerializer.Deserialize<List<JsonElement>>(jsonData);
            return suppliers ?? new List<JsonElement>();
        }

        public void WritesuppliersToJson(List<JsonElement> suppliers)
        {
            var jsonData = JsonSerializer.Serialize(suppliers, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(_filePath, jsonData);
        }
    }
}