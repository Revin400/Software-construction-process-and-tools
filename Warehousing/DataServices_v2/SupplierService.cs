using System;
using System.Collections.Generic;
using System.Linq;

namespace Warehousing.DataServices_v2
{
    public class SupplierService
    {
        private readonly WarehousingContext _context;

        public SupplierService(WarehousingContext context)
        {
            _context = context;
             _context.Database.EnsureCreated();
        }

        public List<Supplier> GetSuppliers() => _context.Suppliers.ToList();

        public Supplier GetSupplierById(int id) => _context.Suppliers.FirstOrDefault(s => s.Id == id);

        public void AddSupplier(Supplier supplier)
        {
            supplier.CreatedAt = DateTime.Now;
            supplier.UpdatedAt = DateTime.Now;
            _context.Suppliers.Add(supplier);
            _context.SaveChanges();
        }

        public void UpdateSupplier(Supplier supplier)
        {
            var existingSupplier = GetSupplierById(supplier.Id);
            if (existingSupplier == null) return;

            existingSupplier.Code = supplier.Code;
            existingSupplier.Name = supplier.Name;
            existingSupplier.Address = supplier.Address;
            existingSupplier.Address_extra = supplier.Address_extra;
            existingSupplier.City = supplier.City;
            existingSupplier.ZipCode = supplier.ZipCode;
            existingSupplier.Province = supplier.Province;
            existingSupplier.Country = supplier.Country;
            existingSupplier.ContactName = supplier.ContactName;
            existingSupplier.ContactPhone = supplier.ContactPhone;
            existingSupplier.Reference = supplier.Reference;
            existingSupplier.UpdatedAt = DateTime.Now;

            _context.SaveChanges();
        }

        public void DeleteSupplier(int id)
        {
            var supplier = GetSupplierById(id);
            if (supplier == null) return;

            _context.Suppliers.Remove(supplier);
            _context.SaveChanges();
        }

    }
}