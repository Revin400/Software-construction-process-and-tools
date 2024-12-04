using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using Warehousing.DataServices_v1;

namespace Warehousing.DataControllers_v1
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class ShipmentController : ControllerBase
    {
        private readonly ShipmentService _shipmentService = new ShipmentService();
        private readonly string _filePath = Path.Combine(Directory.GetCurrentDirectory(), "Datasources", "Shipment.json");

        [HttpGet]
        public IActionResult Get()
        {
            try
            {
                var shipments = _shipmentService.ReadShipmentsFromJson();
                return Ok(shipments);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error reading shipments: {ex.Message}");
            }
        }

        [HttpGet("{id}")]
        public IActionResult Get(int id) 
        {
            try
            {
                var shipments = _shipmentService.ReadShipmentsFromJson();
                if (id < 0 || id >= shipments.Count)
                {
                    return StatusCode(200, $"null");
                }
                return Ok(shipments[id]);
            }
            catch (Exception ex)
            {
                return BadRequest($"Error reading shipments: {ex.Message}");
            }
        }

        [HttpGet("{id}/items")]

        public IActionResult GetItems(int id)
        {
            try
            {
                var shipments = _shipmentService.ReadShipmentsFromJson();
                if (id < 0 || id >= shipments.Count)
                {
                    return StatusCode(200, $"null");
                }
                return Ok(shipments[id - 1].GetProperty("Items"));
            }
            catch (Exception ex)
            {
                return BadRequest($"Error reading shipments: {ex.Message}");
            }
        }

        [HttpPost]
        public IActionResult Post([FromBody] JsonElement shipment)
        {
            try
            {
                var shipments = _shipmentService.ReadShipmentsFromJson();

                var shipmentWithId = JsonSerializer.Serialize(shipment);
                shipments.Add(JsonDocument.Parse(shipmentWithId).RootElement);

                _shipmentService.WriteShipmentsToJson(shipments);
                return StatusCode(201);
            }
            catch (Exception ex)
            {
                return BadRequest($"Error creating shipment: {ex.Message}");
            }
        }

        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody] JsonElement shipment)
        {
            try
            {
                var shipments = _shipmentService.ReadShipmentsFromJson();
                var updatedShipment = JsonSerializer.Serialize(shipment);
                shipments[id - 1] = JsonDocument.Parse(updatedShipment).RootElement;

                _shipmentService.WriteShipmentsToJson(shipments);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest($"Error updating shipments: {ex.Message}");
            }
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {          
            System.IO.File.WriteAllText(_filePath, "[]");
            return Ok();
           
            
        }
    }
}