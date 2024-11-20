using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using Warehousing.DataServices_v1;


namespace Warehousing.DataControllers_v1
{
    [Route("api/orders")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        //private readonly string dataPath;
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
            var order = orders.FirstOrDefault(t => t.Id == orderId);
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
            try
            {
                var inventories = _orderService.ReadOrdersFromJson();
                var existingInventory = inventories.FirstOrDefault(w => w.Id == orderId);
                if (existingInventory == null)
                {
                    return NotFound($"Order with id {orderId} not found");
                }
                existingInventory.Id = order.Id;

                if (inventories.Any(w => w.Id == order.Id && w.Id != orderId))
                {
                    return BadRequest($"Order with id {order.Id} already exists.");
                }

                existingInventory.OrderDate = order.OrderDate;

                existingInventory.UpdatedAt = DateTime.Now;

                _orderService.WriteOrdersToJson(inventories);
                return Ok(existingInventory);
            }
            catch (Exception ex)
            {
                return BadRequest($"Error updating inventory: {ex.Message}");
            }
        }

        [HttpPut("{orderId}/items")]
        public ActionResult UpdateItemsInOrder(int orderId, [FromBody] List<OrderItems> items)
        {
            var data = _orderService.ReadOrdersFromJson();
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
            var data = _orderService.ReadOrdersFromJson();
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
            try
            {
                var orders = _orderService.ReadOrdersFromJson();
                var order = orders.FirstOrDefault(w => w.Id == orderId);
                if (order == null)
                {
                    return NotFound($"Order with id {orderId} not found");
                }
                orders.Remove(order);
                _orderService.WriteOrdersToJson(orders);
                return Ok(order);
            }
            catch (Exception ex)
            {
                return BadRequest($"Error deleting order: {ex.Message}");
            }
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