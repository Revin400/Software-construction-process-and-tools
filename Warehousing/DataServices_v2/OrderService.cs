using System;
using System.Collections.Generic;
using System.Linq;



namespace Warehousing.DataServices_v2
{
    public class OrderService
    {
        private readonly WarehousingContext _context;

        public OrderService(WarehousingContext context)
        {
            _context = context;
            _context.Database.EnsureCreated();
        }

        public List<Order> GetOrders()
        {
            return _context.Orders.ToList();
        }

        public Order GetOrderById(int id)
        {
            return _context.Orders.FirstOrDefault(o => o.Id == id);
        }

        public List<OrderItems> GetItemsInOrder(int orderId)
        {
            var order = _context.Orders.FirstOrDefault(o => o.Id == orderId);
            return order.Items;
        }

        public List<int> GetOrdersInShipment(int shipmentId)
        {
            return _context.Orders.Where(o => o.ShipmentId == shipmentId).Select(o => o.Id).ToList();
        }

        public List<Order> GetOrdersForClient(int clientId)
        {
            return _context.Orders.Where(o => o.ShipTo == clientId || o.BillTo == clientId).ToList();
        }

        public void AddOrder(Order order)
        {
            order.CreatedAt = DateTime.Now;
            order.UpdatedAt = DateTime.Now;
            _context.Orders.Add(order);
            _context.SaveChanges();
        }

        public void UpdateOrder(int id, Order order)
        {
            var existingOrder = _context.Orders.FirstOrDefault(o => o.Id == id);
            if (existingOrder == null) return;

            existingOrder.ShipTo = order.ShipTo;
            existingOrder.BillTo = order.BillTo;
            existingOrder.ShipmentId = order.ShipmentId;
            existingOrder.Items = order.Items;
            existingOrder.UpdatedAt = DateTime.Now;

            _context.Orders.Update(existingOrder);
            _context.SaveChanges();
        }

        public void UpdateOrderItems(int orderId, List<OrderItems> items)
        {
            var order = _context.Orders.FirstOrDefault(o => o.Id == orderId);
            if (order == null) return;

            order.Items = items;
            order.UpdatedAt = DateTime.Now;

            _context.Orders.Update(order);
            _context.SaveChanges(); 
        }

        public void UpdateOrdersInShipment(int shipmentId, List<OrderItems> items)
        {
            var orders = _context.Orders.Where(o => o.ShipmentId == shipmentId).ToList();
            if (orders.Count == 0) return;

            foreach (var order in orders)
            {
                order.Items = items;
                order.UpdatedAt = DateTime.Now;
            }

            _context.Orders.UpdateRange(orders);
            _context.SaveChanges();
        }

        public void DeleteOrder(int id)
        {
            var order = _context.Orders.FirstOrDefault(o => o.Id == id);
            if (order == null) return;

            _context.Orders.Remove(order);
            _context.SaveChanges();
        }
        


    }
}