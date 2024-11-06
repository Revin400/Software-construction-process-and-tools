using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;

public class TransferService
{
    private readonly string _filePath;

    public TransferService(string rootPath)
    {
        _filePath = Path.Combine(rootPath, "transfers.json");
    }

    public List<Transfer> ReadTransfersFromJson()
    {
        if (!File.Exists(_filePath))
        {
            File.WriteAllText(_filePath, "[]");
            return new List<Transfer>();
        }

        var jsonData = File.ReadAllText(_filePath);

        if (string.IsNullOrWhiteSpace(jsonData))
        {
            return new List<Transfer>();
        }
        return JsonSerializer.Deserialize<List<Transfer>>(jsonData) ?? new List<Transfer>();
    }

    public void WriteTransfersToJson(List<Transfer> transfers)
    {
        var jsonData = JsonSerializer.Serialize(transfers, new JsonSerializerOptions { WriteIndented = true });
        File.WriteAllText(_filePath, jsonData);
    }

    public int NextId()
    {
        var transfers = ReadTransfersFromJson();
        return transfers.Any() ? transfers.Max(t => t.Id) + 1 : 1;
    }

    public void AddTransfer(Transfer transfer)
    {
        var transfers = ReadTransfersFromJson();
        transfer.Id = NextId();
        transfer.CreatedAt = DateTime.UtcNow;
        transfer.UpdatedAt = DateTime.UtcNow;
        transfers.Add(transfer);
        WriteTransfersToJson(transfers);
    }

    public bool UpdateTransfer(int transferId, Transfer transfer)
    {
        var transfers = ReadTransfersFromJson();
        var existingTransfer = transfers.FirstOrDefault(t => t.Id == transferId);
        if (existingTransfer == null)
        {
            return false;
        }
        transfer.UpdatedAt = DateTime.UtcNow;
        transfers[transfers.IndexOf(existingTransfer)] = transfer;
        WriteTransfersToJson(transfers);
        return true;
    }

    public bool RemoveTransfer(int transferId)
    {
        var transfers = ReadTransfersFromJson();
        var transfer = transfers.FirstOrDefault(t => t.Id == transferId);
        if (transfer == null)
        {
            return false;
        }
        transfers.Remove(transfer);
        WriteTransfersToJson(transfers);
        return true;
    }
}