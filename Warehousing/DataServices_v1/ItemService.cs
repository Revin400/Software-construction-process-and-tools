using System.Collections.Generic;
using System.Linq;

namespace Warehousing.DataServices_v1
{
public class ItemService
{
    private readonly WarehousingContext _context;

    public ItemService(WarehousingContext context)
    {
        _context = context;
        _context.Database.EnsureCreated();

    }

    public IEnumerable<Item> GetAllItems() => _context.Items.ToList();

    public Item GetItemById(int id) => _context.Items.FirstOrDefault(x => x.Id == id);

    public IEnumerable<Item> GetItemsByItemLine(int itemLineId) =>
        _context.Items.Where(x => x.ItemLineId == itemLineId).ToList();

    public IEnumerable<Item> GetItemsByItemGroup(int itemGroupId) =>
        _context.Items.Where(x => x.ItemGroupId == itemGroupId).ToList();

    public IEnumerable<Item> GetItemsByItemType(int itemTypeId) =>
        _context.Items.Where(x => x.ItemTypeId == itemTypeId).ToList();

    public IEnumerable<Item> GetItemsBySupplier(int supplierId) =>
        _context.Items.Where(x => x.SupplierId == supplierId).ToList();

    public void AddItem(Item item)
    {
        item.CreatedAt = item.UpdatedAt = DateTime.UtcNow;
        _context.Items.Add(item);
        _context.SaveChanges();
    }

    public void UpdateItem(int id, Item updatedItem)
    {
        var existingItem = _context.Items.FirstOrDefault(x => x.Id == id);
        if (existingItem == null) return;

        updatedItem.Id = id;
        updatedItem.CreatedAt = existingItem.CreatedAt;
        updatedItem.UpdatedAt = DateTime.UtcNow;
        _context.Entry(existingItem).CurrentValues.SetValues(updatedItem);
        _context.SaveChanges();
    }

    public bool DeleteItem(int id)
    {
        var item = _context.Items.FirstOrDefault(x => x.Id == id);
        if (item != null)
        {
            _context.Items.Remove(item);
            _context.SaveChanges();
            return true;
        }
        return false;
    }
}
}