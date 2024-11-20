using System.Collections.Generic;
using System.IO;
using System.Text.Json;

public class LocationService
{
    private readonly string _filePath = Path.Combine(Directory.GetCurrentDirectory(), "Datasources", "locations.json");

    public List<Location> ReadLocationsFromJson()
    {
        if (!File.Exists(_filePath))
        {
            File.WriteAllText(_filePath, "[]");  
            return new List<Location>(); 
        }

        var jsonData = File.ReadAllText(_filePath);

        if (string.IsNullOrWhiteSpace(jsonData))
        {
            return new List<Location>();
        }
        return JsonSerializer.Deserialize<List<Location>>(jsonData) ?? new List<Location>();
    }

    public void WriteLocationsToJson(List<Location> locations)
    {
        var jsonData = JsonSerializer.Serialize(locations, new JsonSerializerOptions { WriteIndented = true });
        File.WriteAllText(_filePath, jsonData);
    }

    public int NextId()
    {
        var locations = ReadLocationsFromJson();
        return locations.Any() ? locations.Max(l => l.Id) + 1 : 1; 
    }
}