using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;



namespace Warehousing.DataServices_v2
{
public class OrderService
{
    private readonly string _filePath;

    public OrderService()
    {
        _filePath = Path.Combine(Directory.GetCurrentDirectory(), "Datasources", "orders.json");
    }

    public List<Order> ReadOrdersFromJson()
    {
        if (!File.Exists(_filePath))
        {
            File.WriteAllText(_filePath, "[]");  

            return new List<Order>();
        }

        var jsonData = File.ReadAllText(_filePath);

        if (string.IsNullOrWhiteSpace(jsonData))
        {
            return new List<Order>();
        }
        return JsonSerializer.Deserialize<List<Order>>(jsonData) ?? new List<Order>();
    }

    public void WriteOrdersToJson(List<Order> orders)
    {
        var jsonData = JsonSerializer.Serialize(orders, new JsonSerializerOptions { WriteIndented = true });
        File.WriteAllText(_filePath, jsonData);
    }

    public int NextId()
    {
        var orders = ReadOrdersFromJson();
        return orders.Any() ? orders.Max(o => o.Id) + 1 : 1;
    }
}
}