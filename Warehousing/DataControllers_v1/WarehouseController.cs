using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using Warehousing.DataServices_v1;

namespace Warehousing.DataControllers_v1
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class WarehousesController : ControllerBase
    {
        private readonly WarehouseService _warehouseService = new WarehouseService();
        private readonly string _filePath = Path.Combine(Directory.GetCurrentDirectory(), "Datasources", "warehouses.json");

        [HttpGet]
        public IActionResult GetAllWarehouses()
        {
            try
            {
                var warehouses = _warehouseService.ReadWarehousesFromJson();
                return Ok(warehouses);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error reading warehouses: {ex.Message}");
            }
        }

        [HttpGet("{id}")]
        public IActionResult GetWarehouseById(int id)
        {
            try
            {
                var warehouses = _warehouseService.ReadWarehousesFromJson();
                if (id < 0 || id >= warehouses.Count)
                {
                    return StatusCode(200, $"null");
                }
                return Ok(warehouses[id]);
            }
            catch (Exception ex)
            {
                return BadRequest($"Error reading warehouse: {ex.Message}");
            }
        }

        [HttpPost]
        public IActionResult CreateWarehouse([FromBody] JsonElement warehouse)
        {
            try
            {
                var warehouses = _warehouseService.ReadWarehousesFromJson();

                var warehouseWithId = JsonSerializer.Serialize(warehouse);
                warehouses.Add(JsonDocument.Parse(warehouseWithId).RootElement);

                _warehouseService.WriteWarehousesToJson(warehouses);
                return StatusCode(201);
            }
            catch (Exception ex)
            {
                return BadRequest($"Error creating warehouse: {ex.Message}");
            }
        }

        [HttpPut("{id}")]
        public IActionResult UpdateWarehouse(int id, [FromBody] JsonElement warehouse)
        {
            try
            {
                var warehouses = _warehouseService.ReadWarehousesFromJson();
                var updatedWarehouse = JsonSerializer.Serialize(warehouse);
                warehouses[id - 1] = JsonDocument.Parse(updatedWarehouse).RootElement;

                _warehouseService.WriteWarehousesToJson(warehouses);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest($"Error updating warehouse: {ex.Message}");
            }
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteWarehouse(int id)
        {          
            System.IO.File.WriteAllText(_filePath, "[]");
            return Ok();
           
            
        }
    }
}
