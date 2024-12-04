using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;



namespace Warehousing.DataServices_v1
{
    public class ItemTypeService
    {
        private readonly string FilePath = Path.Combine(Directory.GetCurrentDirectory(), "Datasources","item_types.json");

        public List<JsonElement> ReadItemTypesFromJson()
        {
            if (!File.Exists(FilePath))
            {
                File.WriteAllText(FilePath, "[]");
                return new List<JsonElement>();
            }

            var jsonData = File.ReadAllText(FilePath);
            if (string.IsNullOrWhiteSpace(jsonData))
            {
                return new List<JsonElement>();
            }

            var itemTypes = JsonSerializer.Deserialize<List<JsonElement>>(jsonData);
            return itemTypes ?? new List<JsonElement>();
        }

        public void WriteItemTypesToJson(List<JsonElement> itemTypes)
        {
            var jsonData = JsonSerializer.Serialize(itemTypes, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(FilePath, jsonData);
        }
    }
}