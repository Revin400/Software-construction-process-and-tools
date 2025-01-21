using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualBasic;
using Warehousing.DataServices_v2;


namespace Warehousing.DataControllers_v2
{
    [Route("api/v2/[controller]")]
    [ApiController]
    public class WarehousesController : ControllerBase
    {
        private readonly WarehouseService _warehouseService;

        public WarehousesController(WarehouseService warehouseService)
        {
            _warehouseService = warehouseService;
        }

        [HttpGet]
        public IActionResult GetAllWarehouses()
        {
            return Ok(_warehouseService.GetAllWarehouses());
        }

        [HttpGet("{id}")]
        public IActionResult GetWarehouseById(int id)
        {
            var warehouses = _warehouseService.GetAllWarehouses();
            var warehouse = warehouses.FirstOrDefault(w => w.Id == id);
            if (warehouse == null)
            {
                return NotFound($"Warehouse with id {id} not found");
            }
            return Ok(warehouse);
            
        }

        [HttpPost]
        public IActionResult CreateWarehouse([FromBody] Warehouse warehouse)
        {
            var warehouses = _warehouseService.GetAllWarehouses();
            if (warehouses.Any(w => w.Code == warehouse.Code))
            {
                return BadRequest($"Warehouse with code {warehouse.Code} already exists.");
            }
            warehouse.CreatedAt = DateTime.Now;
            warehouse.UpdatedAt = DateTime.Now;
            _warehouseService.AddWarehouse(warehouse);
            return CreatedAtAction(nameof(GetWarehouseById), new { id = warehouse.Id }, warehouse);
        }


        [HttpPut("{id}")]
        public IActionResult UpdateWarehouse(int id, [FromBody] Warehouse warehouse)
        {
            var warehouses = _warehouseService.GetAllWarehouses();
            var existingWarehouse = warehouses.FirstOrDefault(w => w.Id == id);
            if (existingWarehouse == null)
            {
                return NotFound($"Warehouse with id {id} not found");
            }
            existingWarehouse.Code = warehouse.Code;
            existingWarehouse.Name = warehouse.Name;
            existingWarehouse.Address = warehouse.Address;
            existingWarehouse.Zip = warehouse.Zip;
            existingWarehouse.City = warehouse.City;
            existingWarehouse.Province = warehouse.Province;
            existingWarehouse.Country = warehouse.Country;
            existingWarehouse.Contact = warehouse.Contact;
            existingWarehouse.UpdatedAt = DateTime.Now;
            _warehouseService.UpdateWarehouse(existingWarehouse);
            return Ok(existingWarehouse);
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteWarehouse(int id)
        {
            var warehouse = _warehouseService.GetWarehouseById(id);
            if (warehouse == null)
            {
                return NotFound($"Warehouse with id {id} not found");
            }
            _warehouseService.DeleteWarehouse(id);
            return Ok();
        }
    }
}