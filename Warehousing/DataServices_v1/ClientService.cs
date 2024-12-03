using System.Collections.Generic;
using System.IO;
using System.Text.Json;

namespace Warehousing.DataServices_v1
{
    public class ClientService
    {
        private readonly string _filePath = Path.Combine(Directory.GetCurrentDirectory(), "Datasources", "clients.json");

        public List<JsonElement> ReadClientsFromJson()
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

            var clients = JsonSerializer.Deserialize<List<JsonElement>>(jsonData);
            return clients ?? new List<JsonElement>();
        }

        public void WriteClientsToJson(List<JsonElement> clients)
        {
            var jsonData = JsonSerializer.Serialize(clients, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(_filePath, jsonData);
        }
    }
}