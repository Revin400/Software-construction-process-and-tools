using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
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
        private readonly WarehouseService _warehouseService;

        public OrdersController(OrderService orderService, ShipmentService shipmentService, ClientService clientService,
        InventoryService inventoryService, WarehouseService warehouseService)
        {
            _orderService = orderService;
            _shipmentService = shipmentService;
            _clientService = clientService;
            _inventoryService = inventoryService;
            _warehouseService = warehouseService;
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

        [HttpPost]
        public ActionResult AddOrder([FromBody] Order order)
        {
            var orders = _orderService.GetOrders();
            if (orders.Any(o => o.Reference == order.Reference && o.Id != order.Id))
            {
                return BadRequest("Order with the same reference already exists");
            }

            var clients = _clientService.GetAllClients();
            if (!clients.Any(c => c.Id == order.ShipTo) || !clients.Any(c => c.Id == order.BillTo))
            {
                return BadRequest("Invalid client id");
            }

            var inventories = _inventoryService.GetAllInventories();
            foreach (var item in order.Items)
            {
                var inventory = inventories.FirstOrDefault(i => i.ItemId == item.ItemId);
                if (inventory == null)
                {
                    return BadRequest("Invalid item id \n these are the valid item ids: " + string.Join(", ", inventories.Select(i => i.ItemId)));
                }

                // Subtract the ordered amount from the inventory
                inventory.TotalOnHand -= item.Amount;
                inventory.TotalAvailable -= item.Amount;
                inventory.TotalOrdered += item.Amount;
                _inventoryService.UpdateInventory(inventory);
            }

            var warehouses = _warehouseService.GetAllWarehouses();
            if (!warehouses.Any(w => w.Id == order.WarehouseId))
            {
                return BadRequest("Invalid warehouse id");
            }

            order.CreatedAt = DateTime.Now;
            order.UpdatedAt = DateTime.Now;
            _orderService.AddOrder(order);
            return CreatedAtAction(nameof(Get), new { orderId = order.Id }, order);
        }

        [HttpPut("{orderId}")]
        public ActionResult UpdateOrder(int orderId, [FromBody] Order order)
        {
            var existingOrder = _orderService.GetOrderById(orderId);
            if (existingOrder == null)
            {
                return NotFound();
            }

            var inventories = _inventoryService.GetAllInventories();
            foreach (var existingItem in existingOrder.Items)
            {
                var inventory = inventories.FirstOrDefault(i => i.ItemId == existingItem.ItemId);
                if (inventory != null)
                {
                    // Revert the previous order amount
                    inventory.TotalOnHand += existingItem.Amount;
                    inventory.TotalAvailable += existingItem.Amount;
                    _inventoryService.UpdateInventory(inventory);
                }
            }

            foreach (var item in order.Items)
            {
                var inventory = inventories.FirstOrDefault(i => i.ItemId == item.ItemId);
                if (inventory == null)
                {
                    return BadRequest("Invalid item id \n these are the valid item ids: " + string.Join(", ", inventories.Select(i => i.ItemId)));
                }

                // Subtract the new ordered amount from the inventory
                inventory.TotalOnHand -= item.Amount;
                inventory.TotalAvailable -= item.Amount;
                _inventoryService.UpdateInventory(inventory);
            }

            var warehouses = _warehouseService.GetAllWarehouses();
            if (!warehouses.Any(w => w.Id == order.WarehouseId))
            {
                return BadRequest("Invalid warehouse id");
            }

            order.UpdatedAt = DateTime.Now;
            _orderService.UpdateOrder(orderId, order);
            return Ok(order);
        }
    }
}