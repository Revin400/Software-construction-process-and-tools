using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;

[Route("api/orders")]
[ApiController]
public class OrdersController : ControllerBase
{
    private readonly string dataPath;
    private List<Order> data;

    public OrdersController(string rootPath, bool isDebug = false)
    {
        dataPath = Path.Combine(rootPath, "orders.json");
        Load(isDebug);
    }

    [HttpGet]
    public ActionResult<IEnumerable<Order>> GetOrders()
    {
        return Ok(data);
    }

    [HttpGet("{orderId}")]
    public ActionResult<Order> GetOrder(int orderId)
    {
        var order = data.FirstOrDefault(o => o.Id == orderId);
        if (order == null)
        {
            return NotFound();
        }
        return Ok(order);
    }

    [HttpGet("{orderId}/items")]
    public ActionResult<IEnumerable<OrderItem>> GetItemsInOrder(int orderId)
    {
        var order = data.FirstOrDefault(o => o.Id == orderId);
        if (order == null)
        {
            return NotFound();
        }
        return Ok(order.Items);
    }

    [HttpGet("shipment/{shipmentId}")]
    public ActionResult<IEnumerable<int>> GetOrdersInShipment(int shipmentId)
    {
        var orders = data.Where(o => o.ShipmentId == shipmentId).Select(o => o.Id);
        return Ok(orders);
    }

    [HttpGet("client/{clientId}")]
    public ActionResult<IEnumerable<Order>> GetOrdersForClient(int clientId)
    {
        var orders = data.Where(o => o.ShipTo == clientId || o.BillTo == clientId);
        return Ok(orders);
    }

    [HttpPost]
    public ActionResult AddOrder([FromBody] Order order)
    {
        order.CreatedAt = DateTime.UtcNow;
        order.UpdatedAt = DateTime.UtcNow;
        data.Add(order);
        Save();
        return CreatedAtAction(nameof(GetOrder), new { orderId = order.Id }, order);
    }

    [HttpPut("{orderId}")]
    public ActionResult UpdateOrder(int orderId, [FromBody] Order order)
    {
        var existingOrder = data.FirstOrDefault(o => o.Id == orderId);
        if (existingOrder == null)
        {
            return NotFound();
        }
        order.UpdatedAt = DateTime.UtcNow;
        data[data.IndexOf(existingOrder)] = order;
        Save();
        return NoContent();
    }

    [HttpPut("{orderId}/items")]
    public ActionResult UpdateItemsInOrder(int orderId, [FromBody] List<OrderItem> items)
    {
        var order = data.FirstOrDefault(o => o.Id == orderId);
        if (order == null)
        {
            return NotFound();
        }
        order.Items = items;
        UpdateOrder(orderId, order);
        return NoContent();
    }

    [HttpPut("shipment/{shipmentId}")]
    public ActionResult UpdateOrdersInShipment(int shipmentId, [FromBody] List<int> orders)
    {
        var packedOrders = data.Where(o => o.ShipmentId == shipmentId).Select(o => o.Id).ToList();
        foreach (var orderId in packedOrders)
        {
            if (!orders.Contains(orderId))
            {
                var order = data.FirstOrDefault(o => o.Id == orderId);
                order.ShipmentId = -1;
                order.OrderStatus = "Scheduled";
                UpdateOrder(orderId, order);
            }
        }
        foreach (var orderId in orders)
        {
            var order = data.FirstOrDefault(o => o.Id == orderId);
            order.ShipmentId = shipmentId;
            order.OrderStatus = "Packed";
            UpdateOrder(orderId, order);
        }
        return NoContent();
    }

    [HttpDelete("{orderId}")]
    public ActionResult RemoveOrder(int orderId)
    {
        var order = data.FirstOrDefault(o => o.Id == orderId);
        if (order == null)
        {
            return NotFound();
        }
        data.Remove(order);
        Save();
        return NoContent();
    }

    private void Load(bool isDebug)
    {
        if (isDebug)
        {
            data = new List<Order>
            {
                new Order
                {
                    Id = 1,
                    SourceId = 101,
                    OrderDate = DateTime.Parse("2024-05-01T13:45:00Z"),
                    RequestDate = DateTime.Parse("2024-05-05T00:00:00Z"),
                    Reference = "ORD001",
                    ReferenceExtra = "First bulk order",
                    OrderStatus = "Packed",
                    Notes = "Please ensure timely delivery.",
                    ShippingNotes = "Handle with care.",
                    PickingNotes = "Verify items before dispatch.",
                    WarehouseId = 3,
                    ShipTo = 1,
                    BillTo = 2,
                    ShipmentId = 1,
                    TotalAmount = 5000.00m,
                    TotalDiscount = 100.00m,
                    TotalTax = 475.00m,
                    TotalSurcharge = 30.00m,
                    CreatedAt = DateTime.Parse("2024-05-01T13:45:00Z"),
                    UpdatedAt = DateTime.Parse("2024-05-02T09:30:00Z"),
                    Items = new List<OrderItem>
                    {
                        new OrderItem { ItemId = 1, Amount = 50 },
                        new OrderItem { ItemId = 2, Amount = 30 }
                    }
                },
                new Order
                {
                    Id = 2,
                    SourceId = 102,
                    OrderDate = DateTime.Parse("2024-04-25T11:00:00Z"),
                    RequestDate = DateTime.Parse("2024-04-30T00:00:00Z"),
                    Reference = "ORD002",
                    ReferenceExtra = "Urgent office supplies",
                    OrderStatus = "Delivered",
                    Notes = "Deliver between 9 AM and 5 PM.",
                    ShippingNotes = "Use secondary entrance.",
                    PickingNotes = "Double-check item quality.",
                    WarehouseId = 5,
                    ShipTo = 2,
                    BillTo = 1,
                    ShipmentId = 2,
                    TotalAmount = 1500.00m,
                    TotalDiscount = 50.00m,
                    TotalTax = 142.50m,
                    TotalSurcharge = 10.00m,
                    CreatedAt = DateTime.Parse("2024-04-25T11:00:00Z"),
                    UpdatedAt = DateTime.Parse("2024-04-26T08:15:00Z"),
                    Items = new List<OrderItem>
                    {
                        new OrderItem { ItemId = 1, Amount = 20 },
                        new OrderItem { ItemId = 2, Amount = 10 }
                    }
                }
            };
        }
        else
        {
            using (var reader = new StreamReader(dataPath))
            {
                var json = reader.ReadToEnd();
                data = JsonSerializer.Deserialize<List<Order>>(json);
            }
        }
    }

    private void Save()
    {
        using (var writer = new StreamWriter(dataPath))
        {
            var json = JsonSerializer.Serialize(data);
            writer.Write(json);
        }
    }
}