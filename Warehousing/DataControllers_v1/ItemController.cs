using Microsoft.AspNetCore.Mvc;
using Warehousing.DataServices_v1;


namespace Warehousing.DataControllers_v1
{
    [ApiController]
    [Route("api/v1/[controller]")]
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
            return Ok(item);
        }

        
        [HttpGet("itemLine/{itemLineId}")]
        public ActionResult<IEnumerable<Item>> GetItemsByItemLine(int itemLineId)
        {
            var items = _itemService.GetItemsByItemLine(itemLineId);
            return Ok(items);
        }

        [HttpGet("itemGroup/{itemGroupId}")]
        public ActionResult<IEnumerable<Item>> GetItemsByItemGroup(int itemGroupId)
        {
            var items = _itemService.GetItemsByItemGroup(itemGroupId);
            return Ok(items);
        }

        [HttpGet("itemType/{itemTypeId}")]
        public ActionResult<IEnumerable<Item>> GetItemsByItemType(int itemTypeId)
        {
            var items = _itemService.GetItemsByItemType(itemTypeId);
            return Ok(items);
        }

        [HttpGet("supplier/{supplierId}")]
        public ActionResult<IEnumerable<Item>> GetItemsBySupplier(int supplierId)
        {
            var items = _itemService.GetItemsBySupplier(supplierId);
            return Ok(items);
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
            return Ok();
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteItem(int id)
        {

            _itemService.DeleteItem(id);
            return Ok();
        }
    }
}