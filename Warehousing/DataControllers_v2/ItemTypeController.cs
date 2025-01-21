using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Collections.Generic;
using Warehousing.DataServices_v2;


namespace Warehousing.DataControllers_v2
{
    [ApiController]
    [Route("api/v2/[controller]")]
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
            var itemTypes = _service.GetAllItemTypes();
            if (itemTypes.Any(il => il.Name == newItemType.Name))
            {
                return BadRequest($"ItemType with Name {newItemType.Name} already exists");
            }

            _service.AddItemType(newItemType);
            return CreatedAtAction(nameof(GetItemTypeById), new { id = newItemType.Id }, newItemType);
        }

        [HttpPut("{id}")]
        public IActionResult UpdateItemType(int id, [FromBody] ItemType updatedItemType)
        {
            var itemTypes = _service.GetAllItemTypes();

            if (itemTypes.Any(iT => iT.Name == updatedItemType.Name && iT.Id != id))
            {
                return BadRequest($"ItemType with Name {updatedItemType.Name} already exists");
            }


            var itemType = _service.GetItemTypeById(id);
            if (itemType == null) return NotFound();

            _service.UpdateItemType(id, updatedItemType);
            return Ok(updatedItemType);
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteItemType(int id)
        {
            var itemType = _service.GetItemTypeById(id);
            if (itemType == null) return NotFound();
            
            _service.DeleteItemType(id);
            return Ok();
        }
    }
}
