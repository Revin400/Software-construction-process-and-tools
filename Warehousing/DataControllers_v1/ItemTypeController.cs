using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using Warehousing.DataServices_v1;
using System.Text.Json;


namespace Warehousing.DataControllers_v1
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class ItemTypeController : ControllerBase
    {
        private readonly ItemTypeService _service = new ItemTypeService();

        private readonly string FilePath = Path.Combine(Directory.GetCurrentDirectory(), "Datasources", "item_types.json");


        [HttpGet]
        public IActionResult GetAllItemTypes()
        {
            try
            {
                var itemTypes = _service.ReadItemTypesFromJson();
                return Ok(itemTypes);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error reading item types: {ex.Message}");
            }
        }

        [HttpGet("{id}")]
        public IActionResult GetItemTypeById(int id)
        {
            try
            {
                var itemTypes = _service.ReadItemTypesFromJson();
                if (id < 0 || id >= itemTypes.Count)
                {
                    return StatusCode(200, $"null");
                }
                return Ok(itemTypes[id-1]);
            }
            catch (Exception ex)
            {
                return BadRequest($"Error reading item types: {ex.Message}");
            }
        }

        [HttpPost]
        public IActionResult AddItemType([FromBody] ItemType newItemType)
        {
            try
            {
                var itemTypes = _service.ReadItemTypesFromJson();
                var itemTypeWithId = JsonSerializer.Serialize(newItemType);
                itemTypes.Add(JsonDocument.Parse(itemTypeWithId).RootElement);

                _service.WriteItemTypesToJson(itemTypes);
                return StatusCode(201);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error adding item type: {ex.Message}");
            }
        }

        [HttpPut("{id}")]
        public IActionResult UpdateItemType(int id, [FromBody] JsonElement itemType)
        {  
            try
            {
                var itemTypes = _service.ReadItemTypesFromJson();
                var updatedItemType = JsonSerializer.Serialize(itemType);
                itemTypes[id - 1] = JsonDocument.Parse(updatedItemType).RootElement;

                _service.WriteItemTypesToJson(itemTypes);
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error updating item type: {ex.Message}");
            }
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteItemType(int id)
        {
            System.IO.File.WriteAllText(FilePath, "[]");
            return Ok(); 
        }
    }
}