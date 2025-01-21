using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using Warehousing.DataServices_v2;


namespace Warehousing.DataControllers_v2
{
    [ApiController]
    [Route("api/v2/ItemLine")]
    public class ItemLineController : ControllerBase
    {
        private readonly ItemLineService _service;

        public ItemLineController(ItemLineService service)
        {
            _service = service;
        }

        [HttpGet]
        public ActionResult<IEnumerable<ItemLine>> GetAllItemLines()
        {
            return Ok(_service.GetAllItemLines());
        }

        [HttpGet("{id}")]
        public ActionResult<ItemLine> GetItemLineById(int id)
        {
            var itemLine = _service.GetItemLineById(id);
            if (itemLine == null)
                return NotFound();
            return Ok(itemLine);
        }

        [HttpPost]
        public IActionResult Post([FromBody] ItemLine itemLine)
        {
           var itemlines = _service.GetAllItemLines();
           if(itemlines.Any(il => il.Name == itemLine.Name))
           {
               return BadRequest($"ItemLine with Name {itemLine.Name} already exists");
           }

            _service.AddItemLine(itemLine);
            return CreatedAtAction(nameof(GetItemLineById), new { id = itemLine.Id }, itemLine);
        }


        [HttpPut("{id}")]
        public IActionResult UpdateItemLine(int id, [FromBody] ItemLine updatedItemLine)
        {
            var itemline = _service.GetItemLineById(id);
            if (itemline == null) return NotFound();

            var itemlines = _service.GetAllItemLines();
            if(itemlines.Any(il => il.Name == updatedItemLine.Name && il.Id != id))
            {
                return BadRequest($"ItemLine with Name {itemline.Name} already exists");
            }

            _service.UpdateItemLine(id, updatedItemLine);
            return Ok(updatedItemLine);
        }

        [HttpDelete("{id}")]
        public IActionResult RemoveItemLine(int id)
        {
            _service.RemoveItemLine(id);
            return Ok("ItemLine removed");
        }
    }
}