using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using Warehousing.DataServices_v1;

namespace Warehousing.DataServices_v1
{
    public class ItemGroupService
    {
        private readonly string _filePath = Path.Combine(Directory.GetCurrentDirectory(), "Datasources", "item_groups.json");

        public List<JsonElement> GetAllItemGroups()
        {
            return ReadItemGroupsFromJson();
        }

        public JsonElement? GetItemGroupById(int id)
        {
            var itemGroups = ReadItemGroupsFromJson();
            if (id <= 0 || id > itemGroups.Count)
            {
                return null;
            }
            return itemGroups[id - 1];
        }

        public void AddItemGroup(JsonElement itemGroup)
        {
            var itemGroups = ReadItemGroupsFromJson();
            itemGroups.Add(itemGroup);
            WriteItemGroupsToJson(itemGroups);
        }

        public bool UpdateItemGroup(int id, JsonElement itemGroup)
        {
            var itemGroups = ReadItemGroupsFromJson();
            if (id <= 0 || id > itemGroups.Count)
            {
                return false;
            }
            var updatedItemGroup = JsonSerializer.Serialize(itemGroup);
            itemGroups[id - 1] = JsonDocument.Parse(updatedItemGroup).RootElement;
            WriteItemGroupsToJson(itemGroups);
            return true;
        }

        public bool DeleteItemGroup(int id)
        {
            var itemGroups = ReadItemGroupsFromJson();
            if (id <= 0 || id > itemGroups.Count)
            {
                return false;
            }
            
            WriteItemGroupsToJson(itemGroups);
            return true;
        }

        private List<JsonElement> ReadItemGroupsFromJson()
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

        private void WriteItemGroupsToJson(List<JsonElement> itemGroups)
        {
            var jsonData = JsonSerializer.Serialize(itemGroups, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(_filePath, jsonData);
        }
    }
}
