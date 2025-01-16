using Castle.Components.DictionaryAdapter.Xml;
using Microsoft.AspNetCore.Mvc;
using Warehousing.DataServices_v2;


namespace Warehousing.DataControllers_v2
{
    [ApiController]
    [Route("api/v2/[controller]")]
    public class ItemsController : ControllerBase
    {
        private readonly ItemService _itemService;
        private readonly ItemLineService _itemLineService;
        private readonly ItemGroupService _itemGroupService;

        public ItemsController(ItemService itemService, ItemLineService itemLineService, ItemGroupService itemGroupService)
        {
            _itemService = itemService;
            _itemLineService = itemLineService;
            _itemGroupService = itemGroupService;

        }

        [HttpGet]
        public IActionResult GetAllItems() => Ok(_itemService.GetAllItems());

        [HttpGet("{id}")]
        public IActionResult GetItemById(string id)
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
        public IActionResult UpdateItem(string id, [FromBody] Item updatedItem)
        {
            var items = _itemService.GetAllItems();
            if (!items.Any(x => x.Id == id)) return NotFound();

            if(items.Any(x => x.Code == updatedItem.Code && x.Id != updatedItem.Id)) return BadRequest("Code already exists");

            _itemService.UpdateItem(id, updatedItem);
            return Ok(updatedItem);
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteItem(string id)
        {

            var action = _itemService.DeleteItem(id);
            if (action) return Ok();
            return NotFound();
        }

        [HttpGet("itemline/{itemLineId}")]
        public IActionResult GetItemsByItemLineID(int itemLineId)
        {
            var items = _itemService.GetAllItems();
            if (items.Any(x => x.ItemLineId == itemLineId))
            {
                return Ok(_itemService.GetItemsByItemLine(itemLineId));
            }
            return NotFound();
        }


        [HttpGet("itemgroup/{itemGroupId}")]
        public IActionResult GetItemsByItemGroupID(int itemGroupId)
        {
            var items = _itemService.GetAllItems();
            if (items.Any(x => x.ItemGroupId == itemGroupId))
            {
                return Ok(_itemService.GetItemsByItemGroup(itemGroupId));
            }
            return NotFound();
        }
        
        [HttpGet("itemType/{itemTypeId}")]
        public IActionResult GetItemsByItemTypeID(int itemTypeId)
        {
            var items = _itemService.GetAllItems();
            if (items.Any(x => x.ItemTypeId == itemTypeId))
            {
                return Ok(_itemService.GetItemsByItemType(itemTypeId));
            }
            return NotFound();
        }

        [HttpGet("supplier/{supplierId}")]
        public IActionResult GetItemsBySupplierID(int supplierId)
        {
            var items = _itemService.GetAllItems();
            if (items.Any(x => x.SupplierId == supplierId))
            {
                return Ok(_itemService.GetItemsBySupplier(supplierId));
            }
            return NotFound();
        }
    }
}