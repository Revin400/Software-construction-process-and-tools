using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;

[Route("api/inventories")]
[ApiController]
public class InventoriesController : ControllerBase
{
    private readonly List<Inventory> data;

    public InventoriesController()
    {
        data = new List<Inventory>(); // Initialize with your data source
    }

    [HttpGet]
    public ActionResult<IEnumerable<Inventory>> GetInventories()
    {
        return Ok(data);
    }

    [HttpGet("{inventoryId}")]
    public ActionResult<Inventory> GetInventory(int inventoryId)
    {
        var inventory = data.FirstOrDefault(i => i.Id == inventoryId);
        if (inventory == null)
        {
            return NotFound();
        }
        return Ok(inventory);
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

        foreach (var item in data)
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
    public ActionResult AddInventory([FromBody] Inventory inventory)
    {
        inventory.CreatedAt = GetTimestamp();
        inventory.UpdatedAt = GetTimestamp();
        data.Add(inventory);
        return CreatedAtAction(nameof(GetInventory), new { inventoryId = inventory.Id }, inventory);
    }

    [HttpPut("{inventoryId}")]
    public ActionResult UpdateInventory(int inventoryId, [FromBody] Inventory inventory)
    {
        inventory.UpdatedAt = GetTimestamp();
        var index = data.FindIndex(x => x.Id == inventoryId);
        if (index >= 0)
        {
            data[index] = inventory;
            return NoContent();
        }
        return NotFound();
    }

    [HttpDelete("{inventoryId}")]
    public ActionResult RemoveInventory(int inventoryId)
    {
        var inventory = data.FirstOrDefault(i => i.Id == inventoryId);
        if (inventory == null)
        {
            return NotFound();
        }
        data.Remove(inventory);
        return NoContent();
    }

    private DateTime GetTimestamp()
    {
        return DateTime.UtcNow;
    }
}