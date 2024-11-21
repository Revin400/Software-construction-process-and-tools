using Microsoft.AspNetCore.Mvc;
using Warehousing.DataServices_v2;


namespace Warehousing.DataControllers_v2
{
    [Route("api/warehouse")]
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
                var warehouse = warehouses.FirstOrDefault(w => w.Id == id);
                if (warehouse == null)
                {
                    return NotFound($"Warehouse with id {id} not found");
                }
                return Ok(warehouse);
            }
            catch (Exception ex)
            {
                return BadRequest($"Error reading warehouse: {ex.Message}");
            }
        }

        [HttpPost]
        [HttpPost]
        public IActionResult CreateWarehouse([FromBody] Warehouse warehouse)
        {
            try
            {
                var warehouses = _warehouseService.ReadWarehousesFromJson();


                warehouse.Id = _warehouseService.NextId();
                warehouse.CreatedAt = DateTime.Now;
                warehouse.UpdatedAt = DateTime.Now;


                if (warehouses.Any(w => w.Code == warehouse.Code))
                {
                    return BadRequest($"Warehouse with code {warehouse.Code} already exists.");
                }

                warehouses.Add(warehouse);

                _warehouseService.WriteWarehousesToJson(warehouses);

                return Ok(warehouse);
            }
            catch (Exception ex)
            {
                return BadRequest($"Error creating warehouse: {ex.Message}");
            }
        }


        [HttpPut("{id}")]
        public IActionResult UpdateWarehouse(int id, [FromBody] Warehouse warehouse)
        {
            try
            {
                var warehouses = _warehouseService.ReadWarehousesFromJson();
                var existingWarehouse = warehouses.FirstOrDefault(w => w.Id == id);
                if (existingWarehouse == null)
                {
                    return NotFound($"Warehouse with id {id} not found");
                }
                existingWarehouse.Code = warehouse.Code;

                if (warehouses.Any(w => w.Code == warehouse.Code && w.Id != id))
                {
                    return BadRequest($"Warehouse with code {warehouse.Code} already exists.");
                }

                existingWarehouse.Name = warehouse.Name;
                existingWarehouse.Address = warehouse.Address;
                existingWarehouse.Zip = warehouse.Zip;
                existingWarehouse.City = warehouse.City;
                existingWarehouse.Province = warehouse.Province;
                existingWarehouse.Country = warehouse.Country;
                existingWarehouse.Contact = warehouse.Contact;
                existingWarehouse.UpdatedAt = DateTime.Now;
                _warehouseService.WriteWarehousesToJson(warehouses);
                return Ok(existingWarehouse);
            }
            catch (Exception ex)
            {
                return BadRequest($"Error updating warehouse: {ex.Message}");
            }
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteWarehouse(int id)
        {
            try
            {
                var warehouses = _warehouseService.ReadWarehousesFromJson();
                var warehouse = warehouses.FirstOrDefault(w => w.Id == id);
                if (warehouse == null)
                {
                    return NotFound($"Warehouse with id {id} not found");
                }
                warehouses.Remove(warehouse);
                _warehouseService.WriteWarehousesToJson(warehouses);
                return Ok(warehouse);
            }
            catch (Exception ex)
            {
                return BadRequest($"Error deleting warehouse: {ex.Message}");
            }
        }
    }
}