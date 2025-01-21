using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;


namespace Warehousing.DataServices_v2
{
    public class TransferService
    {
        private readonly WarehousingContext _context;

        public TransferService(WarehousingContext context)
        {
            _context = context;
            _context.Database.EnsureCreated();
        }

        public List<Transfer> GetAllTransfers() => _context.Transfers.ToList();

        public Transfer GetTransferById(int id) => _context.Transfers.FirstOrDefault(t => t.Id == id);

        public void AddTransfer(Transfer transfer)
        {
            transfer.CreatedAt = DateTime.Now;
            transfer.UpdatedAt = DateTime.Now;
            _context.Transfers.Add(transfer);
            _context.SaveChanges();
        }

        public void UpdateTransfer(Transfer transfer)
        {
            var existingTransfer = GetTransferById(transfer.Id);
            if (existingTransfer == null) return;

            existingTransfer.Reference = transfer.Reference;
            existingTransfer.TransferFrom = transfer.TransferFrom;
            existingTransfer.TransferTo = transfer.TransferTo;
            existingTransfer.TransferStatus = transfer.TransferStatus;
            existingTransfer.Items = transfer.Items;
            existingTransfer.UpdatedAt = DateTime.Now;
            _context.SaveChanges();
        }

        public void DeleteTransfer(int id)
        {
            var transfer = GetTransferById(id);
            if (transfer == null) return;

            _context.Transfers.Remove(transfer);
            _context.SaveChanges();
        }
    }
}