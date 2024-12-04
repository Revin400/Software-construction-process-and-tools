using System.Text.Json;

namespace Warehousing.DataServices_v1
{
    public class ItemGroupService
    {
        private readonly string _filePath = Path.Combine(Directory.GetCurrentDirectory(), "Datasources", "item_groups.json");

        public List<JsonElement> ReadItemGroupsFromJson()
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

            var itemGroups = JsonSerializer.Deserialize<List<JsonElement>>(jsonData);
            return itemGroups ?? new List<JsonElement>();
        }

        public void WriteItemGroupsToJson(List<JsonElement> itemGroups)
        {
            var jsonData = JsonSerializer.Serialize(itemGroups, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(_filePath, jsonData);
        }
    }
}