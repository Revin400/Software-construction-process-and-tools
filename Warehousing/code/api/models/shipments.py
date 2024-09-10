import json

from models.base import Base
from providers import data_provider

SHIPMENTS = [
    {
        "id": 1,
        "order_id": 1,
        "source_id": 501,
        "order_date": "2024-05-01",
        "request_date": "2024-05-03",
        "shipment_date": "2024-05-05",
        "shipment_type": "Incoming",
        "shipment_status": "Transit",
        "notes": "Express shipment for urgent requirement.",
        "carrier_code": "UPS",
        "carrier_description": "United Parcel Service",
        "service_code": "NextDay",
        "payment_type": "Manual",
        "transfer_mode": "Air",
        "total_package_count": 10,
        "total_package_weight": 200.5,
        "created_at": "2024-05-01T12:00:00Z",
        "updated_at": "2024-05-02T14:00:00Z",
        "items": [
            {
                "item_id": 1,
                "amount": 5
            },
            {
                "item_id": 2,
                "amount": 10
            }
        ]
    },
    {
        "id": 2,
        "order_id": 2,
        "source_id": 502,
        "order_date": "2024-04-28",
        "request_date": "2024-05-01",
        "shipment_date": "2024-05-02",
        "shipment_type": "Outgoing",
        "shipment_status": "Delivered",
        "notes": "Standard shipping, handle with care.",
        "carrier_code": "FEDEX",
        "carrier_description": "Federal Express",
        "service_code": "Ground",
        "payment_type": "Automatic",
        "transfer_mode": "Road",
        "total_package_count": 3,
        "total_package_weight": 45.75,
        "created_at": "2024-04-28T16:30:00Z",
        "updated_at": "2024-05-03T10:00:00Z",
        "items": [
            {
                "item_id": 1,
                "amount": 15
            },
            {
                "item_id": 2,
                "amount": 20
            }
        ]
    }
]

class Shipments(Base):
    def __init__(self, root_path, is_debug=False):
        self.data_path = root_path + "shipments.json"
        self.load(is_debug)

    def get_shipments(self):
        return self.data

    def get_shipment(self, shipment_id):
        for x in self.data:
            if x["id"] == shipment_id:
                return x
        return None

    def get_items_in_shipment(self, shipment_id):
        for x in self.data:
            if x["id"] == shipment_id:
                return x["items"]
        return None

    def add_shipment(self, shipment):
        shipment["created_at"] = self.get_timestamp()
        shipment["updated_at"] = self.get_timestamp()
        self.data.append(shipment)

    def update_shipment(self, shipment_id, shipment):
        shipment["updated_at"] = self.get_timestamp()
        for i in range(len(self.data)):
            if self.data[i]["id"] == shipment_id:
                self.data[i] = shipment
                break
    
    def update_items_in_shipment(self, shipment_id, items):
        shipment = self.get_shipment(shipment_id)
        current = shipment["items"]
        for x in current:
            found = False
            for y in items:
                if x["item_id"] == y["item_id"]:
                    found = True
                    break
            if not found:
                inventories = data_provider.fetch_inventory_pool().get_inventories_for_item(x["item_id"])
                max_ordered = -1
                max_inventory
                for z in inventories:
                    if z["total_ordered"] > max_ordered:
                        max_ordered = z["total_ordered"]
                        max_inventory = z
                max_inventory["total_ordered"] -= x["amount"]
                max_inventory["total_expected"] = y["total_on_hand"] + y["total_ordered"]
                data_provider.fetch_inventory_pool().update_inventory(max_inventory["id"], max_inventory)
        for x in current:
            for y in items:
                if x["item_id"] == y["item_id"]:
                    inventories = data_provider.fetch_inventory_pool().get_inventories_for_item(x["item_id"])
                    max_ordered = -1
                    max_inventory
                    for z in inventories:
                        if z["total_ordered"] > max_ordered:
                            max_ordered = z["total_ordered"]
                            max_inventory = z
                    max_inventory["total_ordered"] += y["amount"] - x["amount"]
                    max_inventory["total_expected"] = y["total_on_hand"] + y["total_ordered"]
                    data_provider.fetch_inventory_pool().update_inventory(max_inventory["id"], max_inventory)
        shipment["items"] = items
        self.update_shipment(shipment_id, shipment)

    def remove_shipment(self, shipment_id):
        for x in self.data:
            if x["id"] == shipment_id:
                self.data.remove(x)

    def load(self, is_debug):
        if is_debug:
            self.data = SHIPMENTS
        else:
            f = open(self.data_path, "r")
            self.data = json.load(f)
            f.close()

    def save(self):
        f = open(self.data_path, "w")
        json.dump(self.data, f)
        f.close()