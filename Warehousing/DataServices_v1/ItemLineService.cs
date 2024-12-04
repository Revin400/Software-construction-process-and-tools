using System.Collections.Generic;
using System.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

namespace Warehousing.DataServices_v1
{
    
    public class ItemLineService
    {

        private readonly string FilePath = Path.Combine(Directory.GetCurrentDirectory(), "Datasources","item_lines.json");

        public List<JsonElement> ReadItemLinesFromJson()
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

            var itemLines = JsonSerializer.Deserialize<List<JsonElement>>(jsonData);
            return itemLines ?? new List<JsonElement>();
        }

        public void WriteItemLinesToJson(List<JsonElement> itemLines)
        {
            var jsonData = JsonSerializer.Serialize(itemLines, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(FilePath, jsonData);
        }
    }

}


