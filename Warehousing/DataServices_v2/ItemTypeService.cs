using System;
using System.Collections.Generic;
using System.Linq;


namespace Warehousing.DataServices_v2
{
public class ItemTypeService
{
    private readonly WarehousingContext _context;

    public ItemTypeService(WarehousingContext context)
    {
        _context = context;
        _context.Database.EnsureCreated();
    }

    public IEnumerable<ItemType> GetAllItemTypes()
    {
        return _context.ItemTypes.ToList();
    }

    public ItemType GetItemTypeById(int id)
    {
        return _context.ItemTypes.FirstOrDefault(it => it.Id == id);
    }

    public void AddItemType(ItemType itemType)
    {
        itemType.CreatedAt = DateTime.UtcNow;
        itemType.UpdatedAt = DateTime.UtcNow;
        _context.ItemTypes.Add(itemType);
        _context.SaveChanges();
    }

    public void UpdateItemType(int id, ItemType updatedItemType)
    {
        var itemType = _context.ItemTypes.FirstOrDefault(it => it.Id == id);
        if (itemType != null)
        {
            itemType.Name = updatedItemType.Name;
            itemType.Description = updatedItemType.Description;
            itemType.UpdatedAt = DateTime.UtcNow;
            _context.SaveChanges();
        }
    }

    public void DeleteItemType(int id)
    {
        var itemType = _context.ItemTypes.FirstOrDefault(it => it.Id == id);
        if (itemType != null)
        {
            _context.ItemTypes.Remove(itemType);
            _context.SaveChanges();
        }
    }
}
}