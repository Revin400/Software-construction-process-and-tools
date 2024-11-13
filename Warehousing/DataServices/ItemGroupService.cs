
using System.Text.Json;

public class ItemGroupService
{
    private const string FilePath = "itemgroup.json";
    private List<ItemGroup> _data;

    public ItemGroupService()
    {
        _data = Load();
    }

    public List<ItemGroup> GetItemGroups()
    {
        return _data;
    }

    public ItemGroup GetItemGroup(int itemGroupId)
    {
        return _data.Find(x => x.Id == itemGroupId);
    }

    public void AddItemGroup(ItemGroup itemGroup)
    {
        itemGroup.CreatedAt = DateTime.UtcNow;
        itemGroup.UpdatedAt = DateTime.UtcNow;
        _data.Add(itemGroup);
        Save();
    }

    public void UpdateItemGroup(int itemGroupId, ItemGroup itemGroup)
    {
        itemGroup.UpdatedAt = DateTime.UtcNow;
        var index = _data.FindIndex(x => x.Id == itemGroupId);
        if (index != -1)
        {
            _data[index] = itemGroup;
            Save();
        }
    }

    public void RemoveItemGroup(int itemGroupId)
    {
        var itemGroup = _data.Find(x => x.Id == itemGroupId);
        if (itemGroup != null)
        {
            _data.Remove(itemGroup);
            Save();
        }
    }

    private List<ItemGroup> Load()
    {
        if (!File.Exists(FilePath))
        {
            return new List<ItemGroup>();
        }

        var json = File.ReadAllText(FilePath);
        return JsonSerializer.Deserialize<List<ItemGroup>>(json) ?? new List<ItemGroup>();
    }

    private void Save()
    {
        var json = JsonSerializer.Serialize(_data,  new JsonSerializerOptions { WriteIndented = true });
        File.WriteAllText(FilePath, json);
    }
}