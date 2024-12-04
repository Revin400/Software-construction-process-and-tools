using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;


namespace Warehousing.DataServices_v1
{

    public class TransferService
    {
        private readonly string _filePath;

        public TransferService()
        {
            _filePath = Path.Combine(Directory.GetCurrentDirectory(), "Datasources", "transfers.json");
        }

        public List<JsonElement> ReadTransfersFromJson()
        {
            if (!File.Exists(_filePath))
            {
                File.WriteAllText(_filePath, "[]");
                return new List<JsonElement>();
            }

            var jsonData = File.ReadAllText(_filePath);
            if (string.IsNullOrWhiteSpace(jsonData))
            {
                return new List<JsonElement>();
            }

            var transfers = JsonSerializer.Deserialize<List<JsonElement>>(jsonData);
            return transfers ?? new List<JsonElement>();
        }

        public void WriteTransfersToJson(List<JsonElement> transfers)
        {
            var jsonData = JsonSerializer.Serialize(transfers, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(_filePath, jsonData);
        }

        public int NextId()

        {

            var transfers = ReadTransfersFromJson();

            return transfers.Any() ? transfers.Max(t => JsonSerializer.Deserialize<Transfer>(t.GetRawText()).Id) + 1 : 1;
        }

/*
        public int NextId()
        {
            var transfers = ReadTransfersFromJson();
            return transfers.Any() ? transfers.Max(t => t.Id) + 1 : 1;
        }
        */
    }
}