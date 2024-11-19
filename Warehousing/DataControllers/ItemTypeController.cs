using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

[ApiController]
[Route("api/itemtype")]
public class ItemTypesController : ControllerBase
{
    private readonly ItemTypeService _service;

    public ItemTypesController()
    {
        _service = new ItemTypeService();
    }

    [HttpGet]
    public ActionResult<List<ItemType>> GetItemTypes()
    {
        return _service.GetItemTypes();
    }

    [HttpGet("{id}")]
    public ActionResult<ItemType> GetItemType(int id)
    {
        var itemType = _service.GetItemType(id);
        if (itemType == null)
        {
            return NotFound();
        }
        return itemType;
    }

    [HttpPost]
    public ActionResult AddItemType(ItemType itemType)
    {
        _service.AddItemType(itemType);
        return Ok();
    }

    [HttpPut("{id}")]
    public ActionResult UpdateItemType(int id, ItemType itemType)
    {
        _service.UpdateItemType(id, itemType);
        return Ok();
    }

    [HttpDelete("{id}")]
    public ActionResult RemoveItemType(int id)
    {
        _service.RemoveItemType(id);
        return Ok();
    }
}