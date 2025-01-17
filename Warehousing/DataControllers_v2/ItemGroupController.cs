using Microsoft.AspNetCore.Mvc;
using Warehousing.DataServices_v2;


namespace Warehousing.DataControllers_v2
{
[ApiController]
[Route("api/v2/itemgroups")]
public class ItemGroupsController : ControllerBase
{
    private readonly ItemGroupService _itemGroupService;

    public ItemGroupsController(ItemGroupService itemGroupService)
    {
        _itemGroupService = itemGroupService;
    }

    [HttpGet]
    public IActionResult GetAllItemGroups() => Ok(_itemGroupService.GetAllItemGroups());

    [HttpGet("{id}")]
    public IActionResult GetItemGroupById(int id)
    {
        var itemGroup = _itemGroupService.GetItemGroupById(id);
        if (itemGroup == null) return NotFound();
        return Ok(itemGroup);
    }

    [HttpPost]
    public IActionResult Post([FromBody] ItemGroup itemGroup)
    {
        var itemGroups = _itemGroupService.GetAllItemGroups();
        if (itemGroups.Any(ig => ig.Name == itemGroup.Name))
        {
            return BadRequest($"ItemGroup with Name {itemGroup.Name} already exists");
        }

        _itemGroupService.AddItemGroup(itemGroup);
        return CreatedAtAction(nameof(GetItemGroupById), new { id = itemGroup.Id }, itemGroup);
    }

    [HttpPut("{id}")]
    public IActionResult UpdateItemGroup(int id, [FromBody] ItemGroup updatedItemGroup)
    {
        var itemGroup = _itemGroupService.GetItemGroupById(id);
        if (itemGroup == null) return NotFound();

        var itemGroups = _itemGroupService.GetAllItemGroups();

        if (itemGroups.Any(ig => ig.Name == updatedItemGroup.Name && ig.Id != id))
        {
            return BadRequest($"ItemGroup with Name {itemGroup.Name} already exists");
        }

        _itemGroupService.UpdateItemGroup(id, updatedItemGroup);
        return Ok(updatedItemGroup);
    }

    [HttpDelete("{id}")]
    public IActionResult DeleteItemGroup(int id)
    {
        var itemGroup = _itemGroupService.GetItemGroupById(id);
        if (itemGroup == null) return NotFound();

        _itemGroupService.DeleteItemGroup(id);
        return Ok();
    }
}
}