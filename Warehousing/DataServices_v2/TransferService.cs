using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;


namespace Warehousing.DataServices_v2
{
public class TransferService
{
    private readonly string _filePath;

    public TransferService()
    {
        _filePath = Path.Combine(Directory.GetCurrentDirectory(), "Datasources", "transfers.json");
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
}
}