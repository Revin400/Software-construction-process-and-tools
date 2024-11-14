using System.Collections.Generic;
using System.IO;
using System.Text.Json;

public class ClientService
{
    private readonly string _filePath = Path.Combine(Directory.GetCurrentDirectory(), "Datasources", "clients.json");

    public List<Client> ReadClientsFromJson()
    {
        if (!File.Exists(_filePath))
        {
            File.WriteAllText(_filePath, "[]");  
            return new List<Client>(); 
        }

        var jsonData = File.ReadAllText(_filePath);

        if (string.IsNullOrWhiteSpace(jsonData))
        {
            return new List<Client>();
        }
        return JsonSerializer.Deserialize<List<Client>>(jsonData) ?? new List<Client>();
    }

    public void WriteClientsToJson(List<Client> clients)
    {
        var jsonData = JsonSerializer.Serialize(clients, new JsonSerializerOptions { WriteIndented = true });
        File.WriteAllText(_filePath, jsonData);
    }

    public int NextId()
    {
        var warehouses = ReadClientsFromJson();
        return warehouses.Any() ? warehouses.Max(w => w.Id) + 1 : 1; 
    }
}