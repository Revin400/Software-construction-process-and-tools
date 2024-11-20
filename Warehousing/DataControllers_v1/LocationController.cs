using Microsoft.AspNetCore.Mvc;

[Route("api/location")]
[ApiController]

public class LocationController : ControllerBase
{
    private readonly LocationService _locationService;

    public LocationController(LocationService locationService)
    {
        _locationService = locationService;
    }

    [HttpGet]
    public IActionResult GetAllLocations()
    {
        try
        {
            var locations = _locationService.ReadLocationsFromJson();
            return Ok(locations);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Error reading locations: {ex.Message}");
        }
    }


    [HttpGet("{id}")]
    public IActionResult GetLocationBywarehouseId(int id)
    {
        try
        {
            var locations = _locationService.ReadLocationsFromJson();
            var location = locations.Find(l => l.Id == id);

            if (location == null)
            {
                return NotFound($"Location with warehouse id {id} not found");
            }

            return Ok(location);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Error reading location: {ex.Message}");
        }
    }

    [HttpPost]
    public IActionResult CreateLocation([FromBody] Location location)
    {
        try
        {
            var locations = _locationService.ReadLocationsFromJson();
            location.Id = _locationService.NextId();
            location.CreatedAt = DateTime.Now;
            location.UpdatedAt = DateTime.Now;
            locations.Add(location);
            
            _locationService.WriteLocationsToJson(locations);
            return Ok(location);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Error creating location: {ex.Message}");
        }
    }

    [HttpPut("{id}")]
    public IActionResult UpdateLocation(int id, [FromBody] Location location)
    {
        try
        {
            var locations = _locationService.ReadLocationsFromJson();
            var existingLocation = locations.Find(l => l.Id == id);

            if (existingLocation == null)
            {
                return NotFound($"Location with id {id} not found");
            }

            existingLocation.Code = location.Code;
            existingLocation.Name = location.Name;
            existingLocation.Warehouse_id = location.Warehouse_id;
            existingLocation.UpdatedAt = DateTime.Now;

            _locationService.WriteLocationsToJson(locations);
            return Ok(existingLocation);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Error updating location: {ex.Message}");
        }
    }

    [HttpDelete("{id}")]
    public IActionResult DeleteLocation(int id)
    {
        try
        {
            var locations = _locationService.ReadLocationsFromJson();
            var location = locations.Find(l => l.Id == id);

            if (location == null)
            {
                return NotFound($"Location with id {id} not found");
            }

            locations.Remove(location);
            _locationService.WriteLocationsToJson(locations);
            return Ok(location);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Error deleting location: {ex.Message}");
        }
    }

}