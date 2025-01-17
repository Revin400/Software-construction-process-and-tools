using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using Warehousing.DataServices_v2;

namespace Warehousing.DataControllers_v2
{
    [Route("api/v2/[controller]")]
    [ApiController]
    public class TransfersController : ControllerBase
    {
        private readonly TransferService _transferService;
        private readonly ItemService _ItemService;

        public TransfersController(TransferService transferService, ItemService itemService)
        {
            _transferService = transferService;
            _ItemService = itemService;
        }

        [HttpGet]
        public IActionResult GetTransfers() => Ok(_transferService.GetAllTransfers());

        [HttpGet("{transferId}")]
        public ActionResult<Transfer> GetTransferById(int transferId)
        {
            var transfer = _transferService.GetTransferById(transferId);
            if (transfer == null) return NotFound();
            return Ok(transfer);
        }

        [HttpPost]
        public IActionResult AddTransfer([FromBody] Transfer transfer)
        {
            var transfers = _transferService.GetAllTransfers();
            if(transfers.Any(t => t.Reference == transfer.Reference))
            {
                return BadRequest("Transfer with the same reference already exists");
            }

            var Items = _ItemService.GetAllItems();
            foreach(var item in transfer.Items)
            {
                if(!Items.Any(i => i.Id == item.ItemId))
                {
                    return BadRequest("Item with id " + item.ItemId + " does not exist");
                }
            }
            _transferService.AddTransfer(transfer);
            return CreatedAtAction(nameof(GetTransferById), new { transferId = transfer.Id }, transfer);
            
        }

        [HttpPut("{transferId}")]
        public ActionResult UpdateTransfer([FromBody] Transfer transfer)
        {
            var existingTransfer = _transferService.GetTransferById(transfer.Id);
            if (existingTransfer == null) return NotFound();
            _transferService.UpdateTransfer(transfer);
            return Ok(transfer);
        }

        [HttpDelete("{transferId}")]
        public ActionResult RemoveTransfer(int transferId)
        {
            var existingTransfer = _transferService.GetTransferById(transferId);
            if (existingTransfer == null) return NotFound();
            _transferService.DeleteTransfer(transferId);
            return Ok();   
        }

    
    }
}