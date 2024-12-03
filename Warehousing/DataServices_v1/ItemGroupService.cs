using System.Text.Json;

namespace Warehousing.DataServices_v1
{
    public class ItemGroupService
    {
        private const string FilePath = "item_group.json";
        private List<ItemGroup> _data;

        public ItemGroupService()
        {
            _data = Load();
        }

        public List<ItemGroup> GetAllItemGroups()
        {
            return _data;
        }

        public ItemGroup GetItemGroupById(int itemGroupId)
        {
            return _data.FirstOrDefault(x => x.Id == itemGroupId);
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
            _data[index] = itemGroup;
            Save();
        }

        public void DeleteItemGroup(int itemGroupId)
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
}