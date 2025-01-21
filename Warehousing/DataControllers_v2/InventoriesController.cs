using Microsoft.AspNetCore.Mvc;
using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using Warehousing.DataServices_v2;
using SharpCompress.Readers;


namespace Warehousing.DataControllers_v2
{
    [Route("api/v2/[controller]")]
    [ApiController]
    public class InventoriesController : ControllerBase
    {
        private readonly InventoryService _inventoryService;
        private readonly LocationService _locationService;
        private readonly ItemService _itemService;

        public InventoriesController(InventoryService inventoryService, LocationService locationService, ItemService itemService)
        {
            _inventoryService = inventoryService;
            _locationService = locationService;
            _itemService = itemService;
        }

        [HttpGet]
        public ActionResult<List<Inventory>> GetInventories()
        {
            return _inventoryService.GetAllInventories();
        }

        [HttpGet("{inventoryId}")]
        public ActionResult<Inventory> GetInventoryById(int inventoryId)
        {
            var inventory = _inventoryService.GetInventoryById(inventoryId);
            if (inventory == null)
            {
                return NotFound();
            }
            return Ok(inventory);            
        }

        [HttpGet("item/{itemId}")]
        public ActionResult<Dictionary<string, decimal>> GetInventoriesForItem(string itemId)
        {

            if (string.IsNullOrEmpty(itemId))
            {
                return BadRequest("Item Id is required");
            }

            var inventories = _inventoryService.GetInventoryByItemId(itemId);

            if(inventories.Any(i => i.ItemId != itemId))
            {
                return NotFound("Item not found");
            }

            var result = new Dictionary<string, decimal>
            {

                { "totalOnHand"    , 0 },
                { "totalExpected"  , 0 },
                { "totalOrdered"   , 0 },
                { "totalAllocated" , 0 },
                { "totalAvailable" , 0 }
            };

            

            foreach (var inventory in inventories)
            {
                result["totalOnHand"] += inventory.TotalOnHand;
                result["totalExpected"] += inventory.TotalExpected;
                result["totalOrdered"] += inventory.TotalOrdered;
                result["totalAllocated"] += inventory.TotalAllocated;
                result["totalAvailable"] += inventory.TotalAvailable;
            }

            return Ok(new { message = $"Inventory for item {itemId}", data = result });
        }

        [HttpPost]
        public IActionResult AddInventory([FromBody] Inventory inventory)
        {
            var inventories = _inventoryService.GetAllInventories();
            var locations = _locationService.GetAllLocations();
            var items = _itemService.GetAllItems();

            if (inventories.Any(i => i.Id == inventory.Id))
            {
                return BadRequest($"Inventory with id {inventory.Id} already exists.");
            }

            if(inventories.Any(i => i.ItemId == inventory.ItemId))
            {
                return BadRequest($"Inventory with item id {inventory.ItemId} already exists.");
            }

            if(!items.Any(i => i.Id == inventory.ItemId))
            {
                return BadRequest("Item not found");
            }

            if(inventory.Locations.Any(lid => !locations.Any(lo => lo.Id == lid)))
            {
                return BadRequest("Location not found");
            }

            inventory.CreatedAt = DateTime.Now;
            inventory.UpdatedAt = DateTime.Now;

            _inventoryService.AddInventory(inventory);
            return CreatedAtAction(nameof(GetInventoryById), new { inventoryId = inventory}, inventory);
        }

        [HttpPut("{inventoryId}")]
        public IActionResult UpdateInventory(int inventoryId, [FromBody] Inventory inventory)
        {
            var inventories = _inventoryService.GetAllInventories();
            var locations = _locationService.GetAllLocations();
            var items = _itemService.GetAllItems();

            if (inventories.Any(i => i.Id != inventoryId))
            {
                return NotFound($"Inventory with id {inventoryId} not found");
            }

            if(inventories.Any(i => i.ItemId == inventory.ItemId && i.Id != inventoryId))
            {
                return BadRequest($"Inventory with item id {inventory.ItemId} already exists.");
            }

            if(inventory.Locations.Any(lid => !locations.Any(lo => lo.Id == lid)))
            {
                return BadRequest("Location not found");
            }

            if(!items.Any(i => i.Id == inventory.ItemId))
            {
                return BadRequest("Item not found");
            }

            inventory.Id = inventoryId;
            inventory.UpdatedAt = DateTime.Now;

            _inventoryService.UpdateInventory(inventory);
            return Ok(inventory);
        }

        [HttpDelete("{inventoryId}")]
        public IActionResult RemoveInventory(int inventoryId)
        {
            var inventory = _inventoryService.GetInventoryById(inventoryId);
            if (inventory == null)
            {
                return NotFound();
            }

            _inventoryService.DeleteInventory(inventoryId);
            return Ok();
        }
    }
}