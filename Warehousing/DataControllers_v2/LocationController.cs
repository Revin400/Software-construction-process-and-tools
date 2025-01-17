using Microsoft.AspNetCore.Mvc;
using Warehousing.DataServices_v2;

namespace Warehousing.DataControllers_v2
{
    [Route("api/v2/[controller]")]
    [ApiController]

    public class LocationController : ControllerBase
    {
        private readonly LocationService _locationService;
        private readonly WarehouseService _warehouseService;

        public LocationController(LocationService locationService, WarehouseService warehouseService)
        {
            _locationService = locationService;
            _warehouseService = warehouseService;
        }

        [HttpGet]
        public IActionResult Get()
        {
            return Ok(_locationService.GetAllLocations());
        }


        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var location = _locationService.GetLocationById(id);
            if (location == null)
            {
                return NotFound();
            }
            return Ok(location);
        }

        [HttpGet("warehouse/{warehouseId}")]
        public IActionResult GetLocationsByWarehouseId(int warehouseId)
        {   
            if (_warehouseService.GetWarehouseById(warehouseId) == null)
            {
                return BadRequest($"Warehouse with id {warehouseId} not found");
            }

            return Ok(_locationService.GetLocationsByWarehouseId(warehouseId));
        }

        [HttpPost]
        public IActionResult Post([FromBody] Location location)
        {
            
                var locations = _locationService.GetAllLocations();
                var warehouses = _warehouseService.GetAllWarehouses();
                
                if (locations.Any(l => l.Code == location.Code && l.Warehouse_id == location.Warehouse_id))
                {
                    return BadRequest($"Location with code {location.Code} already exists");
                }

                if (locations.Any(l => l.Name == location.Name && l.Warehouse_id == location.Warehouse_id))
                {
                    return BadRequest($"Location with name {location.Name} already exists");
                }

                if(!warehouses.Any(w => w.Id == location.Warehouse_id))
                {
                    return BadRequest($"Warehouse with id {location.Warehouse_id} not found");
                }

              

                _locationService.CreateLocation(location);
                return CreatedAtAction(nameof(Get), new { id = location.Id }, location);
            
           
        }

        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody] Location location)
        {
            try
            {
                var locations = _locationService.GetAllLocations();
                var existingLocation = locations.FirstOrDefault(l => l.Id == id);
                if (existingLocation == null)
                {
                    return NotFound($"Location with id {id} not found");
                }

              
                if (_warehouseService.GetWarehouseById(location.Warehouse_id) == null)
                {
                    return BadRequest($"Warehouse with id {location.Warehouse_id} not found");
                }
                

                if (locations.Any(l => l.Code == location.Code && l.Id != id && l.Warehouse_id == location.Warehouse_id))
                {
                    return BadRequest($"Location with code {location.Code} already exists");
                }

                if (locations.Any(l => l.Name == location.Name && l.Id != id && l.Warehouse_id == location.Warehouse_id))
                {
                    return BadRequest($"Location with name {location.Name} already exists");
                }

                location.Id = id;
                _locationService.UpdateLocation(location);
                return Ok(location);
            }
            catch (Exception ex)
            {
                return BadRequest($"Error updating location: {ex.Message}");
            }

        }

        [HttpDelete("{id}")]
        public IActionResult DeleteLocation(int id)
        {
            try
            {
                if (_locationService.GetLocationById(id) == null)
                {
                    return NotFound();
                }
                _locationService.DeleteLocation(id);
                return Ok("Location deleted");
            }
            catch (Exception ex)
            {
                return BadRequest($"Error deleting location: {ex.Message}");
            }
        }

    }
}