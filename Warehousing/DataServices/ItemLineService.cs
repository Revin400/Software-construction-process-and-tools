using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;


public class ItemLineService
{
    private const string FilePath = "item_lines.json";
    private List<ItemLine> _data;

    public ItemLineService()
    {
        _data = Load();
    }

    public List<ItemLine> GetItemLines()
    {
        return _data;
    }

    public ItemLine GetItemLine(int itemLineId)
    {
        return _data.Find(x => x.Id == itemLineId);
    }

    public void AddItemLine(ItemLine itemLine)
    {
        itemLine.CreatedAt = DateTime.UtcNow;
        itemLine.UpdatedAt = DateTime.UtcNow;
        _data.Add(itemLine);
        Save();
    }

    public void UpdateItemLine(int itemLineId, ItemLine itemLine)
    {
        itemLine.UpdatedAt = DateTime.UtcNow;
        var index = _data.FindIndex(x => x.Id == itemLineId);
        if (index != -1)
        {
            _data[index] = itemLine;
            Save();
        }
    }

    public void RemoveItemLine(int itemLineId)
    {
        var itemLine = _data.Find(x => x.Id == itemLineId);
        if (itemLine != null)
        {
            _data.Remove(itemLine);
            Save();
        }
    }

    private List<ItemLine> Load()
    {
        if (!File.Exists(FilePath))
        {
            return new List<ItemLine>();
        }

        var json = File.ReadAllText(FilePath);
        return JsonSerializer.Deserialize<List<ItemLine>>(json) ?? new List<ItemLine>();
    }

    private void Save()
    {
        var json = JsonSerializer.Serialize(_data, new JsonSerializerOptions { WriteIndented = true });
        File.WriteAllText(FilePath, json);
    }
}