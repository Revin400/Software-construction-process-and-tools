import json

from models.base import Base
from providers import data_provider

ORDERS = [
    {
        "id": 1,
        "source_id": 101,
        "order_date": "2024-05-01T13:45:00Z",
        "request_date": "2024-05-05T00:00:00Z",
        "reference": "ORD001",
        "reference_extra": "First bulk order",
        "order_status": "Packed",
        "notes": "Please ensure timely delivery.",
        "shipping_notes": "Handle with care.",
        "picking_notes": "Verify items before dispatch.",
        "warehouse_id": 3,
        "ship_to": 1,
        "bill_to": 2,
        "shipment_id": 1,
        "total_amount": 5000.00,
        "total_discount": 100.00,
        "total_tax": 475.00,
        "total_surcharge": 30.00,
        "created_at": "2024-05-01T13:45:00Z",
        "updated_at": "2024-05-02T09:30:00Z",
        "items": [
            {
                "item_id": 1,
                "amount": 50
            },
            {
                "item_id": 2,
                "amount": 30
            }
        ]
    },
    {
        "id": 2,
        "source_id": 102,
        "order_date": "2024-04-25T11:00:00Z",
        "request_date": "2024-04-30T00:00:00Z",
        "reference": "ORD002",
        "reference_extra": "Urgent office supplies",
        "order_status": "Delivered",
        "notes": "Deliver between 9 AM and 5 PM.",
        "shipping_notes": "Use secondary entrance.",
        "picking_notes": "Double-check item quality.",
        "warehouse_id": 5,
        "ship_to": 2,
        "bill_to": 1,
        "shipment_id": 2,
        "total_amount": 1500.00,
        "total_discount": 50.00,
        "total_tax": 142.50,
        "total_surcharge": 10.00,
        "created_at": "2024-04-25T11:00:00Z",
        "updated_at": "2024-04-26T08:15:00Z",
        "items": [
            {
                "item_id": 1,
                "amount": 20
            },
            {
                "item_id": 2,
                "amount": 10
            }
        ]
    }
]

class Orders(Base):
    def __init__(self, root_path, is_debug=False):
        self.data_path = root_path + "orders.json"
        self.load(is_debug)

    def get_orders(self):
        return self.data

    def get_order(self, order_id):
        for x in self.data:
            if x["id"] == order_id:
                return x
        return None

    def get_items_in_order(self, order_id):
        for x in self.data:
            if x["id"] == order_id:
                return x["items"]
        return None

    def get_orders_in_shipment(self, shipment_id):
        result = []
        for x in self.data:
            if x["shipment_id"] == shipment_id:
                result.append(x["id"])
        return result

    def get_orders_for_client(self, client_id):
        result = []
        for x in self.data:
            if x["ship_to"] == client_id or x["bill_to"] == client_id:
                result.append(x)
        return result

    def add_order(self, order):
        order["created_at"] = self.get_timestamp()
        order["updated_at"] = self.get_timestamp()
        self.data.append(order)

    def update_order(self, order_id, order):
        order["updated_at"] = self.get_timestamp()
        for i in range(len(self.data)):
            if self.data[i]["id"] == order_id:
                self.data[i] = order
                break
    
    def update_items_in_order(self, order_id, items):
        order = self.get_order(order_id)
        current= order["items"]
        for x in current:
            found = False
            for y in items:
                if x["item_id"] == y["item_id"]:
                    found = True
                    break
            if not found:
                inventories = data_provider.fetch_inventory_pool().get_inventories_for_item(x["item_id"])
                min_ordered = 1_000_000_000_000_000_000
                min_inventory
                for z in inventories:
                    if z["total_allocated"] > min_ordered:
                        min_ordered = z["total_allocated"]
                        min_inventory = z
                min_inventory["total_allocated"] -= x["amount"]
                min_inventory["total_expected"] = y["total_on_hand"] + y["total_ordered"]
                data_provider.fetch_inventory_pool().update_inventory(min_inventory["id"], min_inventory)
        for x in current:
            for y in items:
                if x["item_id"] == y["item_id"]:
                    inventories = data_provider.fetch_inventory_pool().get_inventories_for_item(x["item_id"])
                    min_ordered = 1_000_000_000_000_000_000
                    min_inventory
                    for z in inventories:
                        if z["total_allocated"] < min_ordered:
                            min_ordered = z["total_allocated"]
                            min_inventory = z
                min_inventory["total_allocated"] += y["amount"] - x["amount"]
                min_inventory["total_expected"] = y["total_on_hand"] + y["total_ordered"]
                data_provider.fetch_inventory_pool().update_inventory(min_inventory["id"], min_inventory)
        order["items"] = items
        self.update_order(order_id, order)

    def update_orders_in_shipment(self, shipment_id, orders):
        packed_orders = self.get_orders_in_shipment(shipment_id)
        for x in packed_orders:
            if x not in orders:
                order = self.get_order(x)
                order["shipment_id"] = -1
                order["order_status"] = "Scheduled"
                self.update_order(x, order)
        for x in orders:
            order = self.get_order(x)
            order["shipment_id"] = shipment_id
            order["order_status"] = "Packed"
            self.update_order(x, order)

    def remove_order(self, order_id):
        for x in self.data:
            if x["id"] == order_id:
                self.data.remove(x)

    def load(self, is_debug):
        if is_debug:
            self.data = ORDERS
        else:
            f = open(self.data_path, "r")
            self.data = json.load(f)
            f.close()

    def save(self):
        f = open(self.data_path, "w")
        json.dump(self.data, f)
        f.close()