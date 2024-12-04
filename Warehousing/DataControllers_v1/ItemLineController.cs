using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Text.Json;

using Warehousing.DataServices_v1;



namespace Warehousing.DataControllers_v1
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class ItemLineController : ControllerBase
    {
        private readonly ItemLineService _service = new ItemLineService();
        private readonly string FilePath = Path.Combine(Directory.GetCurrentDirectory(), "Datasources", "item_lines.json");

        [HttpGet]
        public IActionResult GetAllItemLines()
        {
            try
            {
                var itemLines = _service.ReadItemLinesFromJson();
                return Ok(itemLines);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error reading item lines: {ex.Message}");
            }
        }

        [HttpGet("{id}")]
        public ActionResult GetItemLineById(int id)
        {
            try 
            {
                var itemLines = _service.ReadItemLinesFromJson();
                if (id < 0 || id >= itemLines.Count)
                {
                    return StatusCode(200, $"null");
                }
                return Ok(itemLines[id-1]);
            }
            catch (Exception ex)
            {
                return BadRequest($"Error reading item lines: {ex.Message}");
            }
        }

        [HttpPost]
        public IActionResult AddItemLine([FromBody] ItemLine itemLine)
        {
            try
            {
                var itemLines = _service.ReadItemLinesFromJson();
                var itemLineWithId = JsonSerializer.Serialize(itemLine);
                itemLines.Add(JsonDocument.Parse(itemLineWithId).RootElement);

                _service.WriteItemLinesToJson(itemLines);
                return StatusCode(201);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error adding item line: {ex.Message}");
            }

        }

        [HttpPut("{id}")]
        public IActionResult UpdateItemLine(int id, [FromBody] ItemLine updatedItemLine)
        {
            try
            {
                var itemLines = _service.ReadItemLinesFromJson();
                var updatedItemLineJson = JsonSerializer.Serialize(updatedItemLine);
                itemLines[id - 1] = JsonDocument.Parse(updatedItemLineJson).RootElement;

                _service.WriteItemLinesToJson(itemLines);
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error updating item line: {ex.Message}");
            }
        }

        [HttpDelete("{id}")]
        public IActionResult RemoveItemLine(int id)
        {
            System.IO.File.WriteAllText(FilePath, "[]");
            return Ok();
        }
    }
}