using Microsoft.AspNetCore.Mvc;
using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;

[Route("api/inventories")]
[ApiController]
public class InventoriesController : ControllerBase
{
    private readonly InventoryService _inventoryService;

    public InventoriesController(InventoryService inventoryService)
    {
        _inventoryService = inventoryService;
    }

    [HttpGet]
    public IActionResult GetInventories()
    {
        try
        {
            var inventories = _inventoryService.ReadInventoriesFromJson();
            return Ok(inventories);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Error reading inventories: {ex.Message}");
        }
    }

    [HttpGet("{inventoryId}")]
    public ActionResult<Inventory> GetInventoryById(int inventoryId)
    {
        try
        {
            var inventories = _inventoryService.ReadInventoriesFromJson();
            var inventory = inventories.FirstOrDefault(t => t.Id == inventoryId);
            if (inventory == null)
            {
                return NotFound($"Inventory with id {inventoryId} not found");
            }
            return Ok(inventory);
        }
        catch (Exception ex)
        {
            return BadRequest($"Error reading inventory: {ex.Message}");
        }
    }

    [HttpGet("item/{itemId}")]
    public ActionResult<Dictionary<string, decimal>> GetInventoriesForItem(int itemId)
    {
        var result = new Dictionary<string, decimal>
        {
            { "total_expected", 0 },
            { "total_ordered", 0 },
            { "total_allocated", 0 },
            { "total_available", 0 }
        };

        var inventories = _inventoryService.ReadInventoriesFromJson();
        foreach (var item in inventories)
        {
            if (item.ItemId == itemId)
            {
                result["total_expected"] += item.TotalExpected;
                result["total_ordered"] += item.TotalOrdered;
                result["total_allocated"] += item.TotalAllocated;
                result["total_available"] += item.TotalAvailable;
            }
        }
        return Ok(result);
    }

    [HttpPost]
    public IActionResult AddInventory([FromBody] Inventory inventory)
    {
        try
        {
            var inventories = _inventoryService.ReadInventoriesFromJson();

           
            inventory.Id = _inventoryService.NextId();
            inventory.CreatedAt = DateTime.Now;
            inventory.UpdatedAt = DateTime.Now;

           
            if (inventories.Any(w => w.Id == inventory.Id))
            {
                return BadRequest($"Inventory with id {inventory.Id} already exists.");
            }

            inventories.Add(inventory);

            _inventoryService.WriteInventoriesToJson(inventories);

            return Ok(inventory);
        }
        catch (Exception ex)
        {
            return BadRequest($"Error creating inventory: {ex.Message}");
        }
    }

    [HttpPut("{inventoryId}")]
    public IActionResult UpdateInventory(int inventoryId, [FromBody] Inventory inventory)
    {
        try
        {
            var inventories = _inventoryService.ReadInventoriesFromJson();
            var existingInventory = inventories.FirstOrDefault(w => w.Id == inventoryId);
            if (existingInventory == null)
            {
                return NotFound($"Inventory with id {inventoryId} not found");
            }
            existingInventory.Id = inventory.Id;

            if (inventories.Any(w => w.Id == inventory.Id && w.Id != inventoryId))
            {
                return BadRequest($"Inventory with id {inventory.Id} already exists.");
            }
     
            existingInventory.ItemId = inventory.ItemId;
            existingInventory.Description = inventory.Description;
            existingInventory.ItemReference = inventory.ItemReference;
            existingInventory.LocationId = inventory.LocationId;
            existingInventory.TotalOnHand = inventory.TotalOnHand;
            existingInventory.TotalExpected = inventory.TotalExpected;
            existingInventory.TotalOrdered = inventory.TotalOrdered;
            existingInventory.TotalAllocated = inventory.TotalAllocated;
            existingInventory.TotalAvailable = inventory.TotalAvailable;

            existingInventory.UpdatedAt = DateTime.Now;

            _inventoryService.WriteInventoriesToJson(inventories);
            return Ok(existingInventory);
        }
        catch (Exception ex)
        {
            return BadRequest($"Error updating inventory: {ex.Message}");
        }
    }

    [HttpDelete("{inventoryId}")]
    public IActionResult RemoveInventory(int inventoryId)
    {
        try
        {
            var inventories = _inventoryService.ReadInventoriesFromJson();
            var inventory = inventories.FirstOrDefault(w => w.Id == inventoryId);
            if (inventory == null)
            {
                return NotFound($"Inventory with id {inventoryId} not found");
            }
            inventories.Remove(inventory);
            _inventoryService.WriteInventoriesToJson(inventories);
            return Ok(inventory);
        }
        catch (Exception ex)
        {
            return BadRequest($"Error deleting inventory: {ex.Message}");
        }
    }
}