using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;


namespace Warehousing.DataServices_v1
{
public class OrderService
{
    private readonly string _filePath;

    public OrderService()
    {
        _filePath = Path.Combine(Directory.GetCurrentDirectory(), "Datasources", "orders.json");
    }

    public List<JsonElement> ReadOrdersFromJson()
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

            var orders = JsonSerializer.Deserialize<List<JsonElement>>(jsonData);
            return orders ?? new List<JsonElement>();
        }

        public void WriteOrdersToJson(List<JsonElement> orders)
        {
            var jsonData = JsonSerializer.Serialize(orders, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(_filePath, jsonData);
        }

        public int NextId()

        {

            var orders = ReadOrdersFromJson();

            return orders.Any() ? orders.Max(t => JsonSerializer.Deserialize<Transfer>(t.GetRawText()).Id) + 1 : 1;
        }

}
}