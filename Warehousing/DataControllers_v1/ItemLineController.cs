using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using Warehousing.DataServices_v1;



namespace Warehousing.DataControllers_v1
{
    [ApiController]
    [Route("api/ItemLine")]
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
        public IActionResult AddItemLine([FromBody] ItemLine itemLine)
        {
            _service.AddItemLine(itemLine);
            return CreatedAtAction(nameof(GetItemLineById), new { id = itemLine.Id }, itemLine);
        }

        [HttpPut("{id}")]
        public IActionResult UpdateItemLine(int id, [FromBody] ItemLine updatedItemLine)
        {
            _service.UpdateItemLine(id, updatedItemLine);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult RemoveItemLine(int id)
        {
            _service.RemoveItemLine(id);
            return NoContent();
        }
    }
}