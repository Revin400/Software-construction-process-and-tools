using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

public class ItemTypeService
{
    private const string FilePath = "item_types.json";
    private List<ItemType> _data;

    public ItemTypeService()
    {
        _data = Load();
    }

    public List<ItemType> GetItemTypes()
    {
        return _data;
    }

    public ItemType GetItemType(int itemTypeId)
    {
        return _data.Find(x => x.Id == itemTypeId);
    }

    public void AddItemType(ItemType itemType)
    {
        itemType.CreatedAt = DateTime.UtcNow;
        itemType.UpdatedAt = DateTime.UtcNow;
        _data.Add(itemType);
        Save();
    }

    public void UpdateItemType(int itemTypeId, ItemType itemType)
    {
        itemType.UpdatedAt = DateTime.UtcNow;
        var index = _data.FindIndex(x => x.Id == itemTypeId);
        if (index != -1)
        {
            _data[index] = itemType;
            Save();
        }
    }

    public void RemoveItemType(int itemTypeId)
    {
        var itemType = _data.Find(x => x.Id == itemTypeId);
        if (itemType != null)
        {
            _data.Remove(itemType);
            Save();
        }
    }

    private List<ItemType> Load()
    {
        if (!File.Exists(FilePath))
        {
            return new List<ItemType>();
        }

        var json = File.ReadAllText(FilePath);
        return JsonSerializer.Deserialize<List<ItemType>>(json) ?? new List<ItemType>();
    }

    private void Save()
    {
        var json = JsonSerializer.Serialize(_data, new JsonSerializerOptions { WriteIndented = true });
        File.WriteAllText(FilePath, json);
    }
}
