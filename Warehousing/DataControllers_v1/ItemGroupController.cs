using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using Warehousing.DataServices_v1;

namespace Warehousing.DataControllers_v1
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class ItemGroupsController : ControllerBase
    {

        private readonly ItemGroupService _itemGroupService = new ItemGroupService();

        [HttpGet]
        public IActionResult GetAllItemGroups()
        {
            try
            {
                var itemGroups = _itemGroupService.GetAllItemGroups();
                return Ok(itemGroups);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error reading item groups: {ex.Message}");
            }
        }

        [HttpGet("{id}")]
        public IActionResult GetItemGroupById(int id)
        {
            try
            {
                var itemGroup = _itemGroupService.GetItemGroupById(id);
                if (itemGroup == null)
                {
                    return NotFound($"Item group with ID {id} not found.");
                }
                return Ok(itemGroup);
            }
            catch (Exception ex)
            {
                return BadRequest($"Error reading item groups: {ex.Message}");
            }
        }

        [HttpPost]
        public IActionResult AddItemGroup([FromBody] JsonElement itemGroup)
        {
            try
            {
                _itemGroupService.AddItemGroup(itemGroup);
                return StatusCode(201);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error adding item group: {ex.Message}");
            }
        }

        [HttpPut("{id}")]
        public IActionResult UpdateItemGroup(int id, [FromBody] JsonElement itemGroup)
        {
            try
            {
                var success = _itemGroupService.UpdateItemGroup(id, itemGroup);
                if (!success)
                {
                    return NotFound($"Item group with ID {id} not found.");
                }
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error updating item group: {ex.Message}");
            }
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteItemGroup(int id)
        {
            try
            {
                var success = _itemGroupService.DeleteItemGroup(id);
                if (!success)
                {
                    return NotFound($"Item group with ID {id} not found.");
                }
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error deleting item group: {ex.Message}");
            }
        }
    }
}