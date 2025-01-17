using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using Warehousing.DataServices_v2;


namespace Warehousing.DataControllers_v2
{
    [Route("api/v2/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly OrderService _orderService;
        private readonly ShipmentService _shipmentService;

        private readonly ClientService _clientService;
        private readonly InventoryService _inventoryService;

        public OrdersController(OrderService orderService, ShipmentService shipmentService, ClientService clientService, InventoryService inventoryService)
        {
            _orderService = orderService;
            _shipmentService = shipmentService;
            _clientService = clientService;
            _inventoryService = inventoryService;
        }
    

        [HttpGet]
        public IActionResult Get()
        {
            return Ok(_orderService.GetOrders());
        }

        [HttpGet("{orderId}")]
        public ActionResult<Order> Get(int orderId)
        {
            var order = _orderService.GetOrderById(orderId);
            if (order == null)
            {
                return NotFound();
            }
            return Ok(order);
        }


        [HttpGet("{orderId}/items")]
        public ActionResult<IEnumerable<OrderItems>> GetItemsInOrder(int orderId)
        {
            var order = _orderService.GetOrderById(orderId);
            if (order == null)
            {
                return NotFound();
            }
            return Ok(_orderService.GetItemsInOrder(orderId));
        }

        [HttpGet("shipment/{shipmentId}")]
        public ActionResult<IEnumerable<int>> GetOrdersInShipment(int shipmentId)
        {
            var shipment = _shipmentService.GetShipmentById(shipmentId);
            if (shipment == null)
            {
                return NotFound();
            }
            return Ok(_orderService.GetOrdersInShipment(shipmentId));
        }

        [HttpGet("client/{clientId}")]
        public ActionResult<IEnumerable<Order>> GetOrdersForClient(int clientId)
        {
            var client = _clientService.GetClientById(clientId);
            if (client == null)
            {
                return NotFound();
            }
            return Ok(_orderService.GetOrdersForClient(clientId));
        }

        [HttpPost]
        public ActionResult AddOrder([FromBody] Order order)
        {
            var orders = _orderService.GetOrders();
            if (orders.Any(o => o.Id == order.Id))
            {
                return BadRequest($"Order with id {order.Id} already exists.");
            }

            var clients = _clientService.GetAllClients();
            if (!clients.Any(c => c.Id == order.ShipTo) || !clients.Any(c => c.Id == order.BillTo))
            {
                return BadRequest("Invalid client id");
            }

            var inventories = _inventoryService.GetAllInventories();
            foreach (var item in order.Items)
            {
                if (!inventories.Any(i => i.ItemId == item.ItemId))
                {
                    return BadRequest("Invalid item id \n these are the valid item ids: " + string.Join(", ", inventories.Select(i => i.ItemId)));
                }
            }

            order.CreatedAt = DateTime.Now;
            order.UpdatedAt = DateTime.Now;
            _orderService.AddOrder(order);
            return CreatedAtAction(nameof(Get), new { orderId = order.Id }, order);
        }

        [HttpPut("{orderId}")]
        public ActionResult UpdateOrder(int orderId, [FromBody] Order order)
        {
            var orders = _orderService.GetOrders();
            if (!orders.Any(o => o.Id == orderId))
            {
                return NotFound();
            }

            var clients = _clientService.GetAllClients();
            if (!clients.Any(c => c.Id == order.ShipTo) || !clients.Any(c => c.Id == order.BillTo))
            {
                return BadRequest("Invalid client id");
            }

            var inventories = _inventoryService.GetAllInventories();
            foreach (var item in order.Items)
            {
                if (!inventories.Any(i => i.ItemId == item.ItemId))
                {
                    return BadRequest("Invalid item id");
                }
            }

            order.UpdatedAt = DateTime.Now;
            _orderService.UpdateOrder(orderId, order);
            return Ok();
        }

        [HttpPut("{orderId}/items")]
        public ActionResult UpdateItemsInOrder(int orderId, [FromBody] List<OrderItems> items)
        {
        var order = _orderService.GetOrderById(orderId);
        if (order == null)
        {
            return NotFound();
        }

        var currentItems = order.Items;
        foreach (var currentItem in currentItems)
        {
            var found = items.Any(i => i.ItemId == currentItem.ItemId);
            if (!found)
            {
                var inventories = _inventoryService.GetInventoryByItemId(currentItem.ItemId);
                var minInventory = inventories.OrderBy(i => i.TotalAllocated).FirstOrDefault();
                if (minInventory != null)
                {
                    minInventory.TotalAllocated -= currentItem.Amount;
                    minInventory.TotalExpected = minInventory.TotalOnHand + minInventory.TotalOrdered;
                    _inventoryService.UpdateInventory(minInventory);
                }
            }
        }

        foreach (var currentItem in currentItems)
        {
            var newItem = items.FirstOrDefault(i => i.ItemId == currentItem.ItemId);
            if (newItem != null)
            {
                var inventories = _inventoryService.GetInventoryByItemId(currentItem.ItemId);
                var minInventory = inventories.OrderBy(i => i.TotalAllocated).FirstOrDefault();
                if (minInventory != null)
                {
                    minInventory.TotalAllocated += newItem.Amount - currentItem.Amount;
                    minInventory.TotalExpected = minInventory.TotalOnHand + minInventory.TotalOrdered;
                    _inventoryService.UpdateInventory(minInventory);
                }
            }
        }

        order.Items = items;
        _orderService.UpdateOrder(orderId, order);
        return Ok();
        
        }

        [HttpPut("shipment/{shipmentId}")]
        public ActionResult UpdateOrdersInShipment(int shipmentId, [FromBody] List<int> orders)
        {
            var packedOrders = _orderService.GetOrdersInShipment(shipmentId);
            foreach (var orderId in packedOrders)
            {
                if (!orders.Contains(orderId))
                {
                    var order = _orderService.GetOrderById(orderId);
                    if (order != null)
                    {
                        order.ShipmentId = -1;
                        order.OrderStatus = "Scheduled";
                        _orderService.UpdateOrder(orderId, order);
                    }
                }
            }

            foreach (var orderId in orders)
            {
                var order = _orderService.GetOrderById(orderId);
                if (order != null)
                {
                    order.ShipmentId = shipmentId;
                    order.OrderStatus = "Packed";
                    _orderService.UpdateOrder(orderId, order);
                }
            }

            return Ok();
        }

        [HttpDelete("{orderId}")]
        public ActionResult RemoveOrder(int orderId)
        {
            var order = _orderService.GetOrderById(orderId);
            if (order == null)
            {
                return NotFound();
            }

            var items = order.Items;
            foreach (var item in items)
            {
                var inventories = _inventoryService.GetInventoryByItemId(item.ItemId);
                var minInventory = inventories.OrderBy(i => i.TotalAllocated).FirstOrDefault();
                if (minInventory != null)
                {
                    minInventory.TotalAllocated -= item.Amount;
                    minInventory.TotalExpected = minInventory.TotalOnHand + minInventory.TotalOrdered;
                    _inventoryService.UpdateInventory(minInventory);
                }
            }

            _orderService.DeleteOrder(orderId);
            return Ok();
        }
    }
}