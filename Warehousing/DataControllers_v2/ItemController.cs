using Microsoft.AspNetCore.Mvc;

namespace Warehousing.DataControllers_v2
{
    [ApiController]
    [Route("api/items")]
    public class ItemsController : ControllerBase
    {
        private readonly ItemService _itemService;

        public ItemsController(ItemService itemService)
        {
            _itemService = itemService;
        }

        [HttpGet]
        public IActionResult GetAllItems() => Ok(_itemService.GetAllItems());

        [HttpGet("{id}")]
        public IActionResult GetItemById(int id)
        {
            var item = _itemService.GetItemById(id);
            if (item == null) return NotFound();
            return Ok(item);
        }

        [HttpPost]
        public IActionResult AddItem([FromBody] Item item)
        {
            _itemService.AddItem(item);
            return CreatedAtAction(nameof(GetItemById), new { id = item.Id }, item);
        }

        [HttpPut("{id}")]
        public IActionResult UpdateItem(int id, [FromBody] Item updatedItem)
        {
            _itemService.UpdateItem(id, updatedItem);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteItem(int id)
        {

            var action = _itemService.DeleteItem(id);
            if (action) return Ok();
            return NotFound();
        }
    }
}