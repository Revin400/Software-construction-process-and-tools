using Microsoft.AspNetCore.Mvc;
using Warehousing.DataServices_v2;

namespace Warehousing.DataControllers_v2
{
    [Route("api/location/v2")]
    [ApiController]

    public class LocationController : ControllerBase
    {
        private readonly LocationService _locationService;

        public LocationController(LocationService locationService)
        {
            _locationService = locationService;
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

        [HttpPost]
        public IActionResult Post([FromBody] Location location)
        {
            try
            {
                var locations = _locationService.GetAllLocations();
                if (locations.Any(l => l.Code == location.Code))
                {
                    return BadRequest($"Location with code {location.Code} already exists");
                }

                if (locations.Any(l => l.Name == location.Name))
                {
                    return BadRequest($"Location with name {location.Name} already exists");
                }

                _locationService.CreateLocation(location);
                return CreatedAtAction(nameof(Get), new { id = location.Id }, location);
            }
            catch (Exception ex)
            {
                return BadRequest($"Error creating location: {ex.Message}");
            }
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

                if (locations.Any(l => l.Code == location.Code && l.Id != id))
                {
                    return BadRequest($"Location with code {location.Code} already exists");
                }

                if (locations.Any(l => l.Name == location.Name && l.Id != id))
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
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest($"Error deleting location: {ex.Message}");
            }
        }

    }
}