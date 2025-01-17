using System;
using System.Collections.Generic;
using System.Linq;




namespace Warehousing.DataServices_v2
{
    public class ShipmentService
    {
        private readonly WarehousingContext _context;

        public ShipmentService(WarehousingContext context)
        {
            _context = context;
            _context.Database.EnsureCreated();
        }

        public List<Shipment> GetShipments() => _context.Shipments.ToList();

        public Shipment GetShipmentById(int shipmentId) => _context.Shipments.FirstOrDefault(s => s.Id == shipmentId);

        public List<ShipmentItem> GetItemsInShipment(int shipmentId)
        {
            var shipment = GetShipmentById(shipmentId);
            return shipment.Items;
        }

        public void AddShipment(Shipment shipment)
        {
            shipment.CreatedAt = DateTime.Now;
            shipment.UpdatedAt = DateTime.Now;
            _context.Shipments.Add(shipment);
            _context.SaveChanges();
        }

        public void UpdateShipment(Shipment shipment)
        {
            var existingShipment = GetShipmentById(shipment.Id);
            if (existingShipment == null) return;


            existingShipment.OrderId = shipment.OrderId;
            existingShipment.SourceId = shipment.SourceId;
            existingShipment.OrderDate = shipment.OrderDate;
            existingShipment.RequestDate = shipment.RequestDate;
            existingShipment.ShipmentType = shipment.ShipmentType;
            existingShipment.ShipmentStatus = shipment.ShipmentStatus;
            existingShipment.Notes = shipment.Notes;
            existingShipment.CarrierCode = shipment.CarrierCode;
            existingShipment.CarrierDescription = shipment.CarrierDescription;
            existingShipment.ServiceCode = shipment.ServiceCode;
            existingShipment.PaymentType = shipment.PaymentType;
            existingShipment.TransferMode = shipment.TransferMode;
            existingShipment.TotalPackageCount = shipment.TotalPackageCount;
            existingShipment.TotalPackageWeight = shipment.TotalPackageWeight;
            existingShipment.UpdatedAt = DateTime.Now;
            existingShipment.Items = shipment.Items;
            _context.SaveChanges();
        }

        public void UpdateItemsInShipment(int ShipmentId, List<ShipmentItem> items)
        {
            var existingShipment = GetShipmentById(ShipmentId);
            if (existingShipment == null) return;

            existingShipment.Items = items;
            _context.SaveChanges();
        }

        public void Delete_Shipment(int shipmentId)
        {
            var shipment = GetShipmentById(shipmentId);
            if (shipment != null)
            {
                _context.Shipments.Remove(shipment);
                _context.SaveChanges();
            }
        }





    }
}