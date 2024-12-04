using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;



namespace Warehousing.DataServices_v1
{
    public class ItemService
    {
        private readonly string FilePath = Path.Combine(Directory.GetCurrentDirectory(), "Datasources","items.json");

        public List<JsonElement> ReadItemsFromJson()
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

            var items = JsonSerializer.Deserialize<List<JsonElement>>(jsonData);
            return items ?? new List<JsonElement>();
        }

        public void WriteItemsToJson(List<JsonElement> items)
        {
            var jsonData = JsonSerializer.Serialize(items, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(FilePath, jsonData);
        }

        
        public JsonElement? GetItemByItemLine(int itemLine)
        {
            var items = ReadItemsFromJson();
            var item = items.FirstOrDefault(item => item.GetProperty("ItemLineId").GetInt32() == itemLine);
            return item.ValueKind == JsonValueKind.Undefined ? (JsonElement?)null : item;
        }
        
        public JsonElement? GetItemByItemGroup(int itemGroup)
        {
            var items = ReadItemsFromJson();
            var item = items.FirstOrDefault(item => item.GetProperty("ItemGroupId").GetInt32() == itemGroup);
            return item.ValueKind == JsonValueKind.Undefined ? (JsonElement?)null : item;

        }
        
        public JsonElement? GetItemByItemType(int itemType)
        {
            var items = ReadItemsFromJson();
            var item = items.FirstOrDefault(item => item.GetProperty("ItemTypeId").GetInt32() == itemType);
            return item.ValueKind == JsonValueKind.Undefined ? (JsonElement?)null : item;
        }

        public JsonElement? GetItemBySupplier(int supplier)
        {
            var items = ReadItemsFromJson();
            var item = items.FirstOrDefault(item => item.GetProperty("SupplierId").GetInt32() == supplier);
            return item.ValueKind == JsonValueKind.Undefined ? (JsonElement?)null : item;
        }
    }
    
}
