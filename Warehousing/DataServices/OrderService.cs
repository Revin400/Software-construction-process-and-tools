using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;

public class OrderService
{
    private readonly string _filePath;

    public OrderService(string filePath)
    {
        _filePath = filePath;
    }

    public List<Order> ReadOrdersFromJson()
    {
        if (!File.Exists(_filePath))
        {
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

    public IEnumerable<Order> GetOrders()
    {
        return ReadOrdersFromJson();
    }

    public Order GetOrder(int orderId)
    {
        return ReadOrdersFromJson().FirstOrDefault(o => o.Id == orderId);
    }

    public void AddOrder(Order order)
    {
        var orders = ReadOrdersFromJson();
        order.Id = NextId();
        order.CreatedAt = DateTime.UtcNow;
        order.UpdatedAt = DateTime.UtcNow;
        orders.Add(order);
        WriteOrdersToJson(orders);
    }

    public bool UpdateOrder(int orderId, Order order)
    {
        var orders = ReadOrdersFromJson();
        var existingOrder = orders.FirstOrDefault(o => o.Id == orderId);
        if (existingOrder == null)
        {
            return false;
        }
        order.UpdatedAt = DateTime.UtcNow;
        orders[orders.IndexOf(existingOrder)] = order;
        WriteOrdersToJson(orders);
        return true;
    }

    public bool RemoveOrder(int orderId)
    {
        var orders = ReadOrdersFromJson();
        var order = orders.FirstOrDefault(o => o.Id == orderId);
        if (order == null)
        {
            return false;
        }
        orders.Remove(order);
        WriteOrdersToJson(orders);
        return true;
    }
}