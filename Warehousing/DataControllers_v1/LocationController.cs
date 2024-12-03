using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using Warehousing.DataServices_v1;

namespace Warehousing.DataControllers_v1
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class LocationController : ControllerBase
    {
        private readonly LocationService _locationService = new LocationService();
        private readonly string _filePath = Path.Combine(Directory.GetCurrentDirectory(), "Datasources", "locations.json");


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

        [HttpGet("warehouse/{id}")]
        public IActionResult GetLocationsInWarehouse(int id)
        {
            try
            {
                var locations = _locationService.ReadLocationsFromJson();
                var locationsInWarehouse = locations.Where(l => l.GetProperty("warehouse_id").GetInt32() == id).ToList();
                if (locationsInWarehouse.Count == 0 || locationsInWarehouse == null|| id < 0 || id >= locations.Count)
                {
                    return StatusCode(200, $"null");
                }

                return Ok(locationsInWarehouse);
            }
            catch 
            {
                return StatusCode(200, "null");
            }
        }


        [HttpGet("{id}")]
        public IActionResult GetlocationById(int id)
        {
            try
            {
                var locations = _locationService.ReadLocationsFromJson();
                if (id < 0 || id >= locations.Count)
                {
                    return StatusCode(200, $"null");
                }
                return Ok(locations[1-id]);
            }
            catch (Exception ex)
            {
                return BadRequest($"Error reading locations: {ex.Message}");
            }
        }

        [HttpPost]
        public IActionResult Createclient([FromBody] JsonElement location)
        {
            try
            {
                var locations = _locationService.ReadLocationsFromJson();

                var locationWithId = JsonSerializer.Serialize(location);
                locations.Add(JsonDocument.Parse(locationWithId).RootElement);

                _locationService.WriteLocationsToJson(locations);
                return StatusCode(201);
            }
            catch (Exception ex)
            {
                return BadRequest($"Error creating locations: {ex.Message}");
            }
        }

        [HttpPut("{id}")]
        public IActionResult Updatelocation(int id, [FromBody] JsonElement location)
        {
            try
            {
                var locations = _locationService.ReadLocationsFromJson();
                var updatedLocations = JsonSerializer.Serialize(location);
                locations[id - 1] = JsonDocument.Parse(updatedLocations).RootElement;

                _locationService.WriteLocationsToJson(locations);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest($"Error updating location: {ex.Message}");
            }
        }

        [HttpDelete("{id}")]
        public IActionResult Deletelocation(int id)
        {
            System.IO.File.WriteAllText(_filePath, "[]");
            return Ok();
        }

    }
}