using System.Text.Json;

public class ItemService
{
    private List<Item> _items;
    private const string FilePath = "items.json";


    public ItemService()
    {
        _items = LoadItems();
    }

    private List<Item> LoadItems()
    {
        if (!File.Exists(FilePath))
        {
            return new List<Item>();
        }

        var json = File.ReadAllText(FilePath);
        return JsonSerializer.Deserialize<List<Item>>(json) ?? new List<Item>();
    }

    private void SaveItems()
    {
        var json = JsonSerializer.Serialize(_items, new JsonSerializerOptions { WriteIndented = true });
        File.WriteAllText(FilePath, json);
    }

    public List<Item> GetItems()
    {
        return _items;
    }
    public Item GetItem(int id)
    {
        return _items.FirstOrDefault(x => x.Id == id);
    }

    public List<int> GetItemsForItemLine(int itemLineId)
    {
        return _items.Where(x => x.ItemLineId == itemLineId).Select(x => x.Id).ToList();
    }

    public List<int> GetItemsForItemGroup(int itemGroupId)
    {
        return _items.Where(x => x.ItemGroupId == itemGroupId).Select(x => x.Id).ToList();
    }

    public List<int> GetItemsForItemType(int itemTypeId)
    {
        return _items.Where(x => x.ItemTypeId == itemTypeId).Select(x => x.Id).ToList();
    }

    public List<Item> GetItemsForSupplier(int supplierId)
    {
        return _items.Where(x => x.SupplierId == supplierId).ToList();
    }

    public void AddItem(Item item)
    {
        item.CreatedAt = DateTime.UtcNow;
        item.UpdatedAt = DateTime.UtcNow;
        _items.Add(item);
        SaveItems();
    }

    public void UpdateItem(int id, Item item)
    {
        var existingItem = _items.FirstOrDefault(x => x.Id == id);
        if (existingItem != null)
        {
            item.UpdatedAt = DateTime.UtcNow;
            _items[_items.IndexOf(existingItem)] = item;
            SaveItems();
        }
    }

    public void RemoveItem(int id)
    {
        var item = _items.FirstOrDefault(x => x.Id == id);
        if (item != null)
        {
            _items.Remove(item);
            SaveItems();
        }
    }
}