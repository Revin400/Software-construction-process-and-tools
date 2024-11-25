using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using Warehousing.DataServices_v1;


namespace Warehousing.DataControllers_v1
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class ItemTypeController : ControllerBase
    {
        private readonly ItemTypeService _service;

        public ItemTypeController(ItemTypeService service)
        {
            _service = service;
        }

        [HttpGet]
        public ActionResult<IEnumerable<ItemType>> GetAllItemTypes()
        {
            return Ok(_service.GetAllItemTypes());
        }

        [HttpGet("{id}")]
        public ActionResult<ItemType> GetItemTypeById(int id)
        {
            var itemType = _service.GetItemTypeById(id);
            if (itemType == null) return NotFound();
            return Ok(itemType);
        }

        [HttpPost]
        public ActionResult AddItemType([FromBody] ItemType newItemType)
        {
            _service.AddItemType(newItemType);
            return CreatedAtAction(nameof(GetItemTypeById), new { id = newItemType.Id }, newItemType);
        }

        [HttpPut("{id}")]
        public IActionResult UpdateItemType(int id, [FromBody] ItemType updatedItemType)
        {
            _service.UpdateItemType(id, updatedItemType);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteItemType(int id)
        {
            _service.DeleteItemType(id);
            return NoContent();
        }
    }
}