using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;

[Route("api/transfers")]
[ApiController]
public class TransfersController : ControllerBase
{
    private readonly string dataPath;
    private List<Transfer> data;

    public TransfersController(string rootPath, bool isDebug = false)
    {
        dataPath = Path.Combine(rootPath, "transfers.json");
    }

    [HttpGet]
    public ActionResult<IEnumerable<Transfer>> GetTransfers()
    {
        return Ok(data);
    }

    [HttpGet("{transferId}")]
    public ActionResult<Transfer> GetTransfer(int transferId)
    {
        var transfer = data.FirstOrDefault(t => t.Id == transferId);
        if (transfer == null)
        {
            return NotFound();
        }
        return Ok(transfer);
    }

    [HttpPost]
    public ActionResult AddTransfer([FromBody] Transfer transfer)
    {
        transfer.CreatedAt = DateTime.UtcNow;
        transfer.UpdatedAt = DateTime.UtcNow;
        data.Add(transfer);
        Save();
        return CreatedAtAction(nameof(GetTransfer), new { transferId = transfer.Id }, transfer);
    }

    [HttpPut("{transferId}")]
    public ActionResult UpdateTransfer(int transferId, [FromBody] Transfer transfer)
    {
        var existingTransfer = data.FirstOrDefault(t => t.Id == transferId);
        if (existingTransfer == null)
        {
            return NotFound();
        }
        transfer.UpdatedAt = DateTime.UtcNow;
        data[data.IndexOf(existingTransfer)] = transfer;
        Save();
        return NoContent();
    }

    [HttpDelete("{transferId}")]
    public ActionResult RemoveTransfer(int transferId)
    {
        var transfer = data.FirstOrDefault(t => t.Id == transferId);
        if (transfer == null)
        {
            return NotFound();
        }
        data.Remove(transfer);
        Save();
        return NoContent();
    }

    private void Save()
    {
        using (var writer = new StreamWriter(dataPath))
        {
            var json = JsonSerializer.Serialize(data);
            writer.Write(json);
        }
    }
}