using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;


[ApiController]
[Route("api/[controller]")]
public class ItemsController : ControllerBase
{
    private readonly ItemService _itemService;

    public ItemsController()
    {
        _itemService = new ItemService();
    }

    [HttpGet]
    public ActionResult<List<Item>> GetItems()
    {
        return _itemService.GetItems();
    }

    [HttpGet("{id}")]
    public ActionResult<Item> GetItem(int id)
    {
        var item = _itemService.GetItem(id);
        if (item == null)
        {
            return NotFound();
        }
        return item;
    }

    [HttpGet("itemLine/{itemLineId}")]
    public ActionResult<List<int>> GetItemsForItemLine(int itemLineId)
    {
        return _itemService.GetItemsForItemLine(itemLineId);
    }

    [HttpGet("itemGroup/{itemGroupId}")]
    public ActionResult<List<int>> GetItemsForItemGroup(int itemGroupId)
    {
        return _itemService.GetItemsForItemGroup(itemGroupId);
    }

    [HttpGet("itemType/{itemTypeId}")]
    public ActionResult<List<int>> GetItemsForItemType(int itemTypeId)
    {
        return _itemService.GetItemsForItemType(itemTypeId);
    }

    [HttpGet("supplier/{supplierId}")]
    public ActionResult<List<Item>> GetItemsForSupplier(int supplierId)
    {
        return _itemService.GetItemsForSupplier(supplierId);
    }

    [HttpPost]
    public ActionResult AddItem(Item item)
    {
        _itemService.AddItem(item);
        return CreatedAtAction(nameof(GetItem), new { id = item.Id }, item);
    }

    [HttpPut("{id}")]
    public ActionResult UpdateItem(int id, Item item)
    {
        _itemService.UpdateItem(id, item);
        return NoContent();
    }

    [HttpDelete("{id}")]
    public ActionResult RemoveItem(int id)
    {
        _itemService.RemoveItem(id);
        return NoContent();
    }
}