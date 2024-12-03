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
        private readonly string FilePath = Path.Combine(Directory.GetCurrentDirectory(), "Datasources", "item_groups.json");

        [HttpGet]
        public IActionResult GetAllItemGroups() 
        {
            try 
            {
                var itemGroups = _itemGroupService.ReadItemGroupsFromJson();
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
                var itemGroups = _itemGroupService.ReadItemGroupsFromJson();
                if (id < 0 || id >= itemGroups.Count)
                {
                    return StatusCode(200, $"null");
                }
                return Ok(itemGroups[id-1]);
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
                var itemGroups = _itemGroupService.ReadItemGroupsFromJson();
                itemGroups.Add(itemGroup);
                _itemGroupService.WriteItemGroupsToJson(itemGroups);
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
                var itemGroups = _itemGroupService.ReadItemGroupsFromJson();
                var updatedItemGroup = JsonSerializer.Serialize(itemGroup);
                itemGroups[id - 1] = JsonDocument.Parse(updatedItemGroup).RootElement;
                _itemGroupService.WriteItemGroupsToJson(itemGroups);
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
         
            System.IO.File.WriteAllText(FilePath, "[]");
            return Ok();
        }
    }
}