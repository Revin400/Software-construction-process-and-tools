using System.Collections.Generic;
using System.Linq;


namespace Warehousing.DataServices_v2
{
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
            var existingItemLine = GetItemLineById(id);
            if (existingItemLine == null) return;

            updatedItemLine.Id = id;
            updatedItemLine.CreatedAt = existingItemLine.CreatedAt;
            updatedItemLine.UpdatedAt = DateTime.UtcNow;
            _context.Entry(existingItemLine).CurrentValues.SetValues(updatedItemLine);
            _context.SaveChanges();
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

}