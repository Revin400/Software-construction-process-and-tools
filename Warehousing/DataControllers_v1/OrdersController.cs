using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using Warehousing.DataServices_v1;


namespace Warehousing.DataControllers_v1
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class OrdersController : ControllerBase
    {
        private readonly string _filepath = "orders.json";
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

        [HttpGet("{orderId}")]
        public ActionResult<Order> GetOrderById(int orderId)
        {
            var orders = _orderService.ReadOrdersFromJson();
            var order = orders.Select(o => JsonSerializer.Deserialize<Order>(o.GetRawText())).FirstOrDefault(t => t.Id == orderId);
            if (order == null)
            {
                return NotFound();
            }
            return Ok(order);
        }

        /* 
            [HttpGet("{orderId}")]
            public ActionResult<Order> GetOrder(int orderId)
            {
                var order = data.Select(o => JsonSerializer.Deserialize<Order>(o.GetRawText())).FirstOrDefault(o => o.Id == orderId);
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
            var order = orders.Select(o => JsonSerializer.Deserialize<Order>(o.GetRawText())).FirstOrDefault(o => o.Id == orderId);
            if (order == null)
            {
                return NotFound();
            }
            return Ok(order.Items);
        }

        [HttpGet("order/{orderId}")]
        public ActionResult<IEnumerable<int>> GetOrdersInOrder(int orderID)
        {
            var orders = _orderService.ReadOrdersFromJson();
            var order = orders.Select(o => JsonSerializer.Deserialize<Order>(o.GetRawText()))
                              .Where(o => o.Id == orderID)
                              .Select(o => o.Id);
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
                var orderJson = JsonSerializer.SerializeToElement(order);
                orders.Add(orderJson);

                _orderService.WriteOrdersToJson(orders);
                return Ok(order);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error creating order: {ex.Message}");
            }
        }

        public IActionResult CreateOrder([FromBody] JsonElement order)
        {
            try
            {
                var orders = _orderService.ReadOrdersFromJson();

                var orderID = JsonSerializer.Serialize(order);
                orders.Add(JsonDocument.Parse(orderID).RootElement);
                _orderService.WriteOrdersToJson(orders);
                return StatusCode(201);
            }
            catch (Exception ex)
            {
                return BadRequest($"Error creating order: {ex.Message}");
            }
        }

        [HttpPut("{orderId}")]
        public ActionResult UpdateOrder(int orderId, [FromBody] Order order)
        {
            try
            {
                var orders = _orderService.ReadOrdersFromJson();
                var existingOrders = orders.Select(o => JsonSerializer.Deserialize<Order>(o.GetRawText())).FirstOrDefault(w => w.Id == orderId);
                if (existingOrders == null)
                {
                    return NotFound($"Order with id {orderId} not found");
                }
                existingOrders.Id = order.Id;

                if (orders.Select(o => JsonSerializer.Deserialize<Order>(o.GetRawText())).Any(w => w.Id == order.Id && w.Id != orderId))
                {
                    return BadRequest($"Order with id {order.Id} already exists.");
                }

                existingOrders.OrderDate = order.OrderDate;

                existingOrders.UpdatedAt = DateTime.Now;

                _orderService.WriteOrdersToJson(orders);
                return Ok(existingOrders);
            }
            catch (Exception ex)
            {
                return BadRequest($"Error updating order: {ex.Message}");
            }
        }

        [HttpPut("{orderId}/items")]
        public ActionResult UpdateItemsInOrder(int orderId, [FromBody] List<OrderItems> items)
        {
            var data = _orderService.ReadOrdersFromJson();
            var order = data.Select(o => JsonSerializer.Deserialize<Order>(o.GetRawText())).FirstOrDefault(o => o.Id == orderId);
            if (order == null)
            {
                return NotFound();
            }
            order.Items = items;
            UpdateOrder(orderId, order);
            return NoContent();
        }

        [HttpPut("order/{orderId}")]
        public ActionResult UpdateOrdersInShipment(int Id, [FromBody] List<int> orders)
        {
            var data = _orderService.ReadOrdersFromJson();
            var packedOrders = data.Select(o => JsonSerializer.Deserialize<Order>(o.GetRawText()))
                                    .Where(o => o.Id == Id)
                                    .Select(o => o.Id)
                                    .ToList();
            foreach (var orderId in packedOrders)
            {
                if (!orders.Contains(orderId))
                {
                    var order = data.Select(o => JsonSerializer.Deserialize<Order>(o.GetRawText())).FirstOrDefault(o => o.Id == orderId);
                    order.Id = -1;
                    order.OrderStatus = "Scheduled";
                    UpdateOrder(orderId, order);
                }
            }
            foreach (var orderId in orders)
            {
                var order = data.Select(o => JsonSerializer.Deserialize<Order>(o.GetRawText())).FirstOrDefault(o => o.Id == orderId);
                order.Id = orderId;
                order.OrderStatus = "Packed";
                UpdateOrder(orderId, order);
            }
            return NoContent();
        }







/*
    private void Save()
    {
        using (var writer = new StreamWriter(dataPath))
        {
            var data = _orderService.ReadOrdersFromJson();
            var json = JsonSerializer.Serialize(data);
            writer.Write(json);
        }
    }
}
*//*
    private void Save()
    {
        using (var writer = new StreamWriter(dataPath))
        {
            var data = _orderService.ReadOrdersFromJson();
            var json = JsonSerializer.Serialize(data);
            writer.Write(json);
        }
    }
}
*//*
    private void Save()
    {
        using (var writer = new StreamWriter(dataPath))
        {
            var data = _orderService.ReadOrdersFromJson();
            var json = JsonSerializer.Serialize(data);
            writer.Write(json);
        }
    }
}
*//*
    private void Save()
    {
        using (var writer = new StreamWriter(dataPath))
        {
            var data = _orderService.ReadOrdersFromJson();
            var json = JsonSerializer.Serialize(data);
            writer.Write(json);
        }
    }
}
*//*
    private void Save()
    {
        using (var writer = new StreamWriter(dataPath))
        {
            var data = _orderService.ReadOrdersFromJson();
            var json = JsonSerializer.Serialize(data);
            writer.Write(json);
        }
    }
}
*//*
    private void Save()
    {
        using (var writer = new StreamWriter(dataPath))
        {
            var data = _orderService.ReadOrdersFromJson();
            var json = JsonSerializer.Serialize(data);
            writer.Write(json);
        }
    }
}
*//*
    private void Save()
    {
        using (var writer = new StreamWriter(dataPath))
        {
            var data = _orderService.ReadOrdersFromJson();
            var json = JsonSerializer.Serialize(data);
            writer.Write(json);
        }
    }
}
*//*
    private void Save()
    {
        using (var writer = new StreamWriter(dataPath))
        {
            var data = _orderService.ReadOrdersFromJson();
            var json = JsonSerializer.Serialize(data);
            writer.Write(json);
        }
    }
}
*//*
    private void Save()
    {
        using (var writer = new StreamWriter(dataPath))
        {
            var data = _orderService.ReadOrdersFromJson();
            var json = JsonSerializer.Serialize(data);
            writer.Write(json);
        }
    }
}
*//*
    private void Save()
    {
        using (var writer = new StreamWriter(dataPath))
        {
            var data = _orderService.ReadOrdersFromJson();
            var json = JsonSerializer.Serialize(data);
            writer.Write(json);
        }
    }
}
*/





        [HttpDelete("{orderId}")]
        public ActionResult RemoveOrder(int orderId)
        {
            try
            {
                var orders = _orderService.ReadOrdersFromJson();
                var order = orders.Select(o => JsonSerializer.Deserialize<Order>(o.GetRawText())).FirstOrDefault(w => w.Id == orderId);
                if (order == null)
                {
                    return NotFound($"Order with id {orderId} not found");
                }
                var orderElement = orders.FirstOrDefault(o => JsonSerializer.Deserialize<Order>(o.GetRawText()).Id == orderId);
                if (orderElement.ValueKind != JsonValueKind.Undefined)
                {
                    orders.Remove(orderElement);
                }
                _orderService.WriteOrdersToJson(orders);
                return Ok(order);
            }
            catch (Exception ex)
            {
                return BadRequest($"Error deleting order: {ex.Message}");
            }
        }

        [HttpDelete("{orderId}")]
        public IActionResult DeleteOrder(int id)
        {          
            System.IO.File.WriteAllText(_filepath, "[]");
            return Ok();
        }
    }
}
/*
    private void Save()
    {
        using (var writer = new StreamWriter(dataPath))
        {
            var data = _orderService.ReadOrdersFromJson();
            var json = JsonSerializer.Serialize(data);
            writer.Write(json);
        }
    }
}
*/