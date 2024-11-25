
namespace Warehousing.DataServices_v2
{
    public class ItemGroupService
    {
        private readonly WarehousingContext _context;

        public ItemGroupService(WarehousingContext context)
        {
            _context = context;
            _context.Database.EnsureCreated();
        }

        public IEnumerable<ItemGroup> GetAllItemGroups() => _context.ItemGroups.ToList();

        public ItemGroup GetItemGroupById(int id) => _context.ItemGroups.FirstOrDefault(x => x.Id == id);

        public void AddItemGroup(ItemGroup itemGroup)
        {
            itemGroup.CreatedAt = itemGroup.UpdatedAt = DateTime.UtcNow;
            _context.ItemGroups.Add(itemGroup);
            _context.SaveChanges();
        }

        public void UpdateItemGroup(int id, ItemGroup updatedItemGroup)
        {
            var existingItemGroup = _context.ItemGroups.FirstOrDefault(x => x.Id == id);
            if (existingItemGroup == null) return;

            updatedItemGroup.Id = id;
            updatedItemGroup.CreatedAt = existingItemGroup.CreatedAt;
            updatedItemGroup.UpdatedAt = DateTime.UtcNow;
            _context.Entry(existingItemGroup).CurrentValues.SetValues(updatedItemGroup);
            _context.SaveChanges();
        }

        public void DeleteItemGroup(int id)
        {
            var itemGroup = _context.ItemGroups.FirstOrDefault(x => x.Id == id);
            if (itemGroup != null)
            {
                _context.ItemGroups.Remove(itemGroup);
                _context.SaveChanges();
            }
        }
    }
}