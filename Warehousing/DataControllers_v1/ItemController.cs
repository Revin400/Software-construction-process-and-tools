using Microsoft.AspNetCore.Mvc;
using Warehousing.DataServices_v1;
using System.Text.Json;



namespace Warehousing.DataControllers_v1
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class ItemsController : ControllerBase
    {
        private readonly ItemService _itemService = new ItemService();
        private readonly string FilePath = Path.Combine(Directory.GetCurrentDirectory(), "Datasources", "items.json");

        [HttpGet]
        public IActionResult GetAllItems()
        {
            try
            {
                var items = _itemService.ReadItemsFromJson();
                return Ok(items);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error reading items: {ex.Message}");
            }
        }

        [HttpGet("{id}")]
        public IActionResult GetItemById(int id)
        {
            try
            {
                var items = _itemService.ReadItemsFromJson();
                if (id < 0 || id >= items.Count)
                {
                    return StatusCode(200, $"null");
                }
                return Ok(items[id]);
            }
            catch (Exception ex)
            {
                return BadRequest($"Error reading items: {ex.Message}");
            }
        }

        
        [HttpGet("itemLine/{itemLineId}")]
        public IActionResult GetItemByItemLine(int itemLineId)
        {
            try
            {
                var item = _itemService.GetItemByItemLine(itemLineId);
                if (item.HasValue)
                {
                    return Ok(item.Value);
                }
                else
                {
                    return NotFound("Item not found");
                }
            }
            catch (Exception ex)
            {
                return BadRequest($"Error reading items: {ex.Message}");
            }
        }

        [HttpGet("itemGroup/{itemGroupId}")]
        public IActionResult GetItemByItemGroup(int itemGroupId)
        {
            try
            {
                var item = _itemService.GetItemByItemGroup(itemGroupId);
                if (item.HasValue)
                {
                    return Ok(item.Value);
                }
                else
                {
                    return NotFound("Item not found");
                }
            }
            catch (Exception ex)
            {
                return BadRequest($"Error reading items: {ex.Message}");
            }
        }

        [HttpGet("itemType/{itemTypeId}")]
        public IActionResult GetItemByItemType(int itemTypeId)
        {
            try
            {
                var item = _itemService.GetItemByItemType(itemTypeId);
                if (item.HasValue)
                {
                    return Ok(item.Value);
                }
                else
                {
                    return NotFound("Item not found");
                }
            }
            catch (Exception ex)
            {
                return BadRequest($"Error reading items: {ex.Message}");
            }
        }
        [HttpGet("supplier/{supplierId}")]
        public IActionResult GetItemBySupplier(int supplierId)
        {
            try
            {
                var item = _itemService.GetItemBySupplier(supplierId);
                if (item.HasValue)
                {
                    return Ok(item.Value);
                }
                else
                {
                    return NotFound("Item not found");
                }
            }
            catch (Exception ex)
            {
                return BadRequest($"Error reading items: {ex.Message}");
            }
        }

        [HttpPost]
        public IActionResult AddItem([FromBody] Item item)
        {
            try
            {
                var items = _itemService.ReadItemsFromJson();

                var itemWithId = JsonSerializer.Serialize(item);
                items.Add(JsonDocument.Parse(itemWithId).RootElement);

                _itemService.WriteItemsToJson(items);
                return StatusCode(201);
            }
            catch (Exception ex)
            {
                return BadRequest($"Error creating items: {ex.Message}");
            }
        }

        [HttpPut("{id}")]
        public IActionResult UpdateItem(int id, [FromBody] Item updatedItem)
        {
            try
            {
                var items = _itemService.ReadItemsFromJson();
                var updatedItemJson = JsonSerializer.Serialize(updatedItem);
                items[id - 1] = JsonDocument.Parse(updatedItemJson).RootElement;

                _itemService.WriteItemsToJson(items);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest($"Error updating items: {ex.Message}");
            }
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteItem(int id)
        {          
            System.IO.File.WriteAllText(FilePath, "[]");
            return Ok(); 
        }
        
    }
}