using System.Collections.Generic;
using System.Linq;

public class ItemLineService
{
    private readonly WarehousingContext _context;

    public ItemLineService(WarehousingContext context)
    {
        _context = context;
        _context.Database.EnsureCreated();
    }

    public IEnumerable<ItemLine> GetAllItemLines() => _context.ItemLines.ToList();

    public ItemLine GetItemLineById(int id) => _context.ItemLines.FirstOrDefault(il => il.Id == id);

    public void AddItemLine(ItemLine itemLine)
    {
        itemLine.CreatedAt = DateTime.UtcNow;
        itemLine.UpdatedAt = DateTime.UtcNow;
        _context.ItemLines.Add(itemLine);
        _context.SaveChanges();
    }

    public void UpdateItemLine(int id, ItemLine updatedItemLine)
    {
        var existing = GetItemLineById(id);
        if (existing != null)
        {
            existing.Name = updatedItemLine.Name;
            existing.Description = updatedItemLine.Description;
            existing.UpdatedAt = DateTime.UtcNow;
            _context.SaveChanges();
        }
    }

    public void RemoveItemLine(int id)
    {
        var itemLine = GetItemLineById(id);
        if (itemLine != null)
        {
            _context.ItemLines.Remove(itemLine);
            _context.SaveChanges();
        }
    }
}

