using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualBasic;
using System.Collections.Generic;
using System.Reflection.Metadata.Ecma335;
using Warehousing.DataServices_v2;


namespace Warehousing.DataControllers_v2
{
    [ApiController]
    [Route("api/v2/[controller]")]

    public class ShipmentController : ControllerBase
    {
        private readonly ShipmentService _shipmentService;
        private readonly OrderService _orderService;

        private readonly InventoryService _inventoryService;

        public ShipmentController(ShipmentService shipmentService, InventoryService inventoryService, OrderService orderService)
        {
            _shipmentService = shipmentService;
            _inventoryService = inventoryService;
            _orderService = orderService;
        }


        [HttpGet]
        public IActionResult GetShipments()
        {
            return Ok(_shipmentService.GetShipments());
        }

        [HttpGet("{id}")]
        public IActionResult GetShipment(int id)
        {
            if (_shipmentService.GetShipmentById(id) == null)
            {
                return NotFound();
            }
            return Ok(_shipmentService.GetShipmentById(id));
        }


        [HttpGet("{id}/items")]
        public IActionResult GetShipmentItems(int id)
        {
            if (_shipmentService.GetShipmentById(id) == null)
            {
                return NotFound();
            }

            return Ok(_shipmentService.GetItemsInShipment(id));
        }

        [HttpPost]
        public IActionResult AddShipment(Shipment shipment)
        {
            if (shipment == null)
            {
                return BadRequest();
            }

            var orders = _orderService.GetOrders();
            if (!orders.Any(o => o.Id == shipment.OrderId))
            {
                return BadRequest("Order does not exist");
            }

            var shipments = _shipmentService.GetShipments();
            if (shipments.Any(s => s.OrderId == shipment.OrderId))
            {
                return BadRequest("Shipment already exists");
            }

            var inventories = _inventoryService.GetAllInventories();

            foreach(var item in shipment.Items)
            {
                if(!inventories.Any(i => i.ItemId == item.Id))
                {
                    return BadRequest("Item does not exist in inventory");
                }
            }

            _shipmentService.AddShipment(shipment);
            return CreatedAtAction(nameof(GetShipment), new { id = shipment.Id }, shipment);
        }

        [HttpPut("{id}")]

        public IActionResult UpdateShipment(int id, Shipment shipment)
        {
            var existingShipment = _shipmentService.GetShipmentById(id);
            if (existingShipment == null)
            {
                return NotFound();
            }

            var shipments = _shipmentService.GetShipments();
            if (shipments.Any(s => s.OrderId == shipment.OrderId && s.Id != id))
            {
                return BadRequest("Shipment already exists");
            }

            _shipmentService.UpdateShipment(shipment);
            return Ok(shipment);
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteShipment(int id)
        {
            var shipment = _shipmentService.GetShipmentById(id);
            if (shipment == null)
            {
                return NotFound();
            }

            _shipmentService.Delete_Shipment(id);
            return Ok();
        }

        [HttpPut("{id}/items")]
        public IActionResult UpdateShipmentItems(int id, List<ShipmentItem> items)
        {
            var shipment = _shipmentService.GetShipmentById(id);
            if (shipment == null)
            {
                return NotFound();
            }

            var currentItems = shipment.Items;
            foreach (var currentItem in currentItems)
            {
                var found = false;
                foreach (var newItem in items)
                {
                    if (currentItem.Id == newItem.Id)
                    {
                        found = true;
                        break;
                    }
                }
                if (!found)
                {
                    var inventories = _inventoryService.GetInventoryByItemId(currentItem.Id);
                    var maxOrdered = -1;
                    Inventory maxInventory = null;
                    foreach (var inventory in inventories)
                    {
                        if (inventory.TotalOrdered > maxOrdered)
                        {
                            maxOrdered = inventory.TotalOrdered;
                            maxInventory = inventory;
                        }
                    }
                    if (maxInventory != null)
                    {


                        maxInventory.TotalOrdered -= (int)currentItem.Amount;
                        maxInventory.TotalExpected = inventories.Where(i => i.ItemId == currentItem.Id).Sum(i => i.TotalOnHand) + maxInventory.TotalOrdered;
                        _inventoryService.UpdateInventory(maxInventory);
                    }
                }
            }

            foreach (var currentItem in currentItems)
            {
                foreach (var newItem in items)
                {
                    if (currentItem.Id == newItem.Id)
                    {
                        var inventories = _inventoryService.GetInventoryByItemId(currentItem.Id);
                        var maxOrdered = -1;
                        Inventory maxInventory = null;
                        foreach (var inventory in inventories)
                        {
                            if (inventory.TotalOrdered > maxOrdered)
                            {
                                maxOrdered = inventory.TotalOrdered;
                                maxInventory = inventory;
                            }
                        }
                        if (maxInventory != null)
                        {
                            maxInventory.TotalOrdered += (int)(newItem.Amount - currentItem.Amount);
                            maxInventory.TotalExpected = inventories.Where(i => i.ItemId == currentItem.Id).Sum(i => i.TotalOnHand) + maxInventory.TotalOrdered;
                            _inventoryService.UpdateInventory(maxInventory);
                        }
                    }
                }
            }

            shipment.Items = items;
            _shipmentService.UpdateShipment(shipment);
            return Ok();
        }



    }
}