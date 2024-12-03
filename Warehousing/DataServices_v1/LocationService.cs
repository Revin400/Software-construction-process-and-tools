using System.Collections.Generic;
using System.IO;
using System.Text.Json;

namespace Warehousing.DataServices_v1
{
    public class LocationService
    {
        private readonly string _filePath = Path.Combine(Directory.GetCurrentDirectory(), "Datasources", "locations.json");

        public List<JsonElement> ReadLocationsFromJson()
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

            var locations = JsonSerializer.Deserialize<List<JsonElement>>(jsonData);
            return locations ?? new List<JsonElement>();
        }

        public void WriteLocationsToJson(List<JsonElement> locations)
        {
            var jsonData = JsonSerializer.Serialize(locations, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(_filePath, jsonData);
        }
        
    }
}