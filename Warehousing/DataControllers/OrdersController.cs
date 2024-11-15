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
    private readonly OrderService _orderService;

    public OrdersController(OrderService orderService)
    {
        _orderService = orderService;
    }

    [HttpGet]
    public IActionResult GetOrders()
    {
        try
        {
            var orders = _orderService.ReadOrdersFromJson();
            return Ok(orders);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Error reading orders: {ex.Message}");
        }
    }

/* 
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
*/

    [HttpGet("{orderId}/items")]
    public ActionResult<IEnumerable<OrderItems>> GetItemsInOrder(int orderId)
    {
        var orders = _orderService.ReadOrdersFromJson();
        var order = orders.FirstOrDefault(o => o.Id == orderId);
        if (order == null)
        {
            return NotFound();
        }
        return Ok(order.Items);
    }

    [HttpGet("shipment/{shipmentId}")]
    public ActionResult<IEnumerable<int>> GetOrdersInShipment(int shipmentId)
    {
        var orders = _orderService.ReadOrdersFromJson();
        var order = orders.Where(o => o.ShipmentId == shipmentId).Select(o => o.Id);
        return Ok(order);
    }

    [HttpGet("client/{clientId}")]
    public ActionResult<IEnumerable<Order>> GetOrdersForClient(int clientId)
    {
        var orders = _orderService.ReadOrdersFromJson();
        var order = orders.Where(o => o.ShipTo == clientId || o.BillTo == clientId);
        return Ok(order);
    }

    [HttpPost]
    public ActionResult AddOrder([FromBody] Order order)
    {
        try
        {
            var orders = _orderService.ReadOrdersFromJson();
            order.Id = _orderService.NextId();
            order.CreatedAt = DateTime.Now;
            order.UpdatedAt = DateTime.Now;
            orders.Add(order);
            
            _orderService.WriteOrdersToJson(orders);
            return Ok(order);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Error creating order: {ex.Message}");
        }
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
    public ActionResult UpdateItemsInOrder(int orderId, [FromBody] List<OrderItems> items)
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

    private void Save()
    {
        using (var writer = new StreamWriter(dataPath))
        {
            var json = JsonSerializer.Serialize(data);
            writer.Write(json);
        }
    }
}