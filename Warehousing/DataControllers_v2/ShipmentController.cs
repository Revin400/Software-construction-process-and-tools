using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using Warehousing.DataServices_v2;


namespace Warehousing.DataControllers_v2
{
    [ApiController]
    [Route("api/[controller]")]

    public class ShipmentController : ControllerBase
    {
        private readonly ShipmentService _shipmentService;

        public ShipmentController()
        {
            _shipmentService = new ShipmentService();
        }

        [HttpGet]
        public IActionResult GetShipments()
        {
            return Ok(_shipmentService.ReadshipmentsFromJson());
        }

        [HttpGet("{id}")]
        public IActionResult GetShipment(int id)
        {
            return Ok(_shipmentService.ReadshipmentsFromJson().Find(s => s.Id == id));
        }



        [HttpPost]
        public IActionResult AddShipment(Shipment shipment)
        {

            var shipments = _shipmentService.ReadshipmentsFromJson();
            shipment.Id = _shipmentService.NextId();
            shipments.Add(shipment);
            _shipmentService.WriteShipmentsToJson(shipments);
            return CreatedAtAction(nameof(GetShipment), new { id = shipment.Id }, shipment);
        }

        [HttpPut("{id}")]

        public IActionResult UpdateShipment(int id, Shipment shipment)
        {
            var shipments = _shipmentService.ReadshipmentsFromJson();
            var existingShipment = shipments.Find(s => s.Id == id);

            if (existingShipment == null)
            {
                return NotFound();
            }

            shipment.Id = existingShipment.Id;
            shipments[shipments.IndexOf(existingShipment)] = shipment;
            _shipmentService.WriteShipmentsToJson(shipments);
            return Ok();
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteShipment(int id)
        {
            var shipments = _shipmentService.ReadshipmentsFromJson();
            var existingShipment = shipments.Find(s => s.Id == id);

            if (existingShipment == null)
            {
                return NotFound();
            }

            shipments.Remove(existingShipment);
            _shipmentService.WriteShipmentsToJson(shipments);
            return Ok();
        }


        [HttpGet("{id}/items")]
        public IActionResult GetShipmentItems(int id)
        {
            var shipments = _shipmentService.ReadshipmentsFromJson();
            var shipment = shipments.Find(s => s.Id == id);

            if (shipment == null)
            {
                return NotFound();
            }

            return Ok(shipment.Items);
        }

    }
}