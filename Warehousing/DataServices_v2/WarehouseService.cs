
namespace Warehousing.DataServices_v2
{
    public class WarehouseService
    {

        private readonly WarehousingContext _context;

        public WarehouseService(WarehousingContext context)
        {
            _context = context;
            _context.Database.EnsureCreated();
        }

        public List<Warehouse> GetAllWarehouses() => _context.Warehouses.ToList();

        public Warehouse GetWarehouseById(int id) => _context.Warehouses.FirstOrDefault(w => w.Id == id);

        public void AddWarehouse(Warehouse warehouse)
        {
            warehouse.CreatedAt = DateTime.Now;
            warehouse.UpdatedAt = DateTime.Now;
            _context.Warehouses.Add(warehouse);
            _context.SaveChanges();
        }

        public void UpdateWarehouse(Warehouse warehouse)
        {
            var existingWarehouse = GetWarehouseById(warehouse.Id);
            if (existingWarehouse == null) return;

            existingWarehouse.Code = warehouse.Code;
            existingWarehouse.Name = warehouse.Name;
            existingWarehouse.Address = warehouse.Address;
            existingWarehouse.Zip = warehouse.Zip;
            existingWarehouse.City = warehouse.City;
            existingWarehouse.Province = warehouse.Province;
            existingWarehouse.Country = warehouse.Country;
            existingWarehouse.Contact = warehouse.Contact;
            existingWarehouse.UpdatedAt = DateTime.Now;
    
            _context.SaveChanges();
        }

        public void DeleteWarehouse(int id)
        {
            var warehouse = GetWarehouseById(id);
            if (warehouse == null) return;

            _context.Warehouses.Remove(warehouse);
            _context.SaveChanges();
        }
    }
}