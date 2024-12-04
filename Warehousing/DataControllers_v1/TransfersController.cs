using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using Warehousing.DataServices_v1;


namespace Warehousing.DataControllers_v1
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class TransfersController : ControllerBase
    {
        private readonly string _filepath = "transfers.json";
        private readonly TransferService _transferService;

        public TransfersController(TransferService transferService)
        {
            _transferService = transferService;
        }

        [HttpGet]
        public IActionResult GetTransfers()
        {
            try
            {
                var transfers = _transferService.ReadTransfersFromJson();
                return Ok(transfers);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error reading transfers: {ex.Message}");
            }
        }

        [HttpGet("{transferId}")]
        public ActionResult<Transfer> GetTransferById(int transferId)
        {
            var transfers = _transferService.ReadTransfersFromJson();
            var transfer = transfers.FirstOrDefault(t => t.Id == transferId);
            if (transfer == null)
            {
                return NotFound();
            }
            return Ok(transfer);
        }

        [HttpPost]
        public IActionResult AddTransfer([FromBody] Transfer transfer)
        {
            try
            {
                var locations = _transferService.ReadTransfersFromJson();
                transfer.Id = _transferService.NextId();
                transfer.CreatedAt = DateTime.Now;
                transfer.UpdatedAt = DateTime.Now;
                locations.Add(transfer);

                _transferService.WriteTransfersToJson(locations);
                return Ok(transfer);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error creating transfer: {ex.Message}");
            }
        }

        public IActionResult CreateTransfer([FromBody] JsonElement transfer)
        {
            try
            {
                var transfers = _transferService.ReadTransfersFromJson();

                var TransferID = JsonSerializer.Serialize(transfer);
                var newTransfer = JsonSerializer.Deserialize<Transfer>(TransferID);
                transfers.Add(newTransfer);

                _transferService.WriteTransfersToJson(transfers);
                return StatusCode(201);
            }
            catch (Exception ex)
            {
                return BadRequest($"Error creating transfers: {ex.Message}");
            }
        }

        [HttpPut("{transferId}")]
        public ActionResult UpdateTransfer(int transferId, [FromBody] Transfer transfer)
        {
            try
            {
                var transfers = _transferService.ReadTransfersFromJson();
                var existingTransfer = transfers.Find(l => l.Id == transferId);

                if (existingTransfer == null)
                {
                    return NotFound($"Transfer with id {transferId} not found");
                }

                existingTransfer.Reference = transfer.Reference;
                existingTransfer.TransferFrom = transfer.TransferFrom;
                existingTransfer.TransferTo = transfer.TransferTo;
                existingTransfer.TransferStatus = transfer.TransferStatus;

                existingTransfer.UpdatedAt = DateTime.Now;

                _transferService.WriteTransfersToJson(transfers);
                return Ok(existingTransfer);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error updating transfer: {ex.Message}");
            }
        }

        [HttpDelete("{transferId}")]
        public ActionResult RemoveTransfer(int transferId)
        {
            try
            {
                var transfers = _transferService.ReadTransfersFromJson();
                var transfer = transfers.Find(l => l.Id == transferId);

                if (transfer == null)
                {
                    return NotFound($"Transfer with id {transferId} not found");
                }

                transfers.Remove(transfer);
                _transferService.WriteTransfersToJson(transfers);
                return Ok(transfer);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error deleting transfer: {ex.Message}");
            }
        }

        [HttpDelete("{transferId}")]
        public IActionResult DeleteTransfer(int id)
        {          
            System.IO.File.WriteAllText(_filepath, "[]");
            return Ok();
        }
    }
}