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

        public void UpdateItemType(int id, ItemType itemtype)
        {
            var existingItemType = GetItemTypeById(id);
            if (existingItemType == null) return;

            itemtype.Id = existingItemType.Id;
            existingItemType.Name = itemtype.Name;
            existingItemType.Description = itemtype.Description;
            
            existingItemType.CreatedAt = existingItemType.CreatedAt;
            existingItemType.UpdatedAt = DateTime.UtcNow;
            _context.Entry(existingItemType).CurrentValues.SetValues(itemtype);
            _context.SaveChanges();
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