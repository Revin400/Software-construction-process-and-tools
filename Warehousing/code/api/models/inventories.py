import json

from models.base import Base

INVENTORIES = [
    {
        "id": 1,
        "item_id": 1,
        "description": "High Precision Bearings",
        "item_reference": "REF123",
        "location_id": 1,
        "total_on_hand": 150,
        "total_expected": 200,
        "total_ordered": 50,
        "total_allocated": 100,
        "total_available": 50,
        "created_at": "2024-03-20T10:00:00Z",
        "updated_at": "2024-03-21T11:00:00Z"
    },
    {
        "id": 2,
        "item_id": 1,
        "description": "High Precision Bearings",
        "item_reference": "REF123",
        "location_id": 2,
        "total_on_hand": 150,
        "total_expected": 200,
        "total_ordered": 50,
        "total_allocated": 100,
        "total_available": 50,
        "created_at": "2024-03-20T10:00:00Z",
        "updated_at": "2024-03-21T11:00:00Z"
    },
    {
        "id": 3,
        "item_id": 2,
        "description": "Stainless Steel Bolts",
        "item_reference": "REF456",
        "location_id": 1,
        "total_on_hand": 500,
        "total_expected": 550,
        "total_ordered": 100,
        "total_allocated": 200,
        "total_available": 300,
        "created_at": "2024-04-01T09:00:00Z",
        "updated_at": "2024-04-02T10:00:00Z"
    },
    {
        "id": 4,
        "item_id": 2,
        "description": "Stainless Steel Bolts",
        "item_reference": "REF456",
        "location_id": 2,
        "total_on_hand": 500,
        "total_expected": 550,
        "total_ordered": 100,
        "total_allocated": 200,
        "total_available": 300,
        "created_at": "2024-04-01T09:00:00Z",
        "updated_at": "2024-04-02T10:00:00Z"
    }
]

class Inventories(Base):
    def __init__(self, root_path, is_debug=False):
        self.data_path = root_path + "inventories.json"
        self.load(is_debug)

    def get_inventories(self):
        return self.data
    
    def get_inventory(self, inventory_id):
        for x in self.data:
            if x["id"] == inventory_id:
                return x
        return None

    def get_inventories_for_item(self, item_id):
        result = []
        for x in self.data:
            if x["item_id"] == item_id:
                result.append(x)
        return result

    def get_inventory_totals_for_item(self, item_id):
        result = {
            "total_expected": 0,
            "total_ordered": 0,
            "total_allocated": 0,
            "total_available": 0
        }
        for x in self.data:
            if x["item_id"] == item_id:
                result["total_expected"] += x["total_expected"]
                result["total_ordered"] += x["total_ordered"]
                result["total_allocated"] += x["total_allocated"]
                result["total_available"] += x["total_available"]
        return result

    def add_inventory(self, inventory):
        inventory["created_at"] = self.get_timestamp()
        inventory["updated_at"] = self.get_timestamp()
        self.data.append(inventory)

    def update_inventory(self, inventory_id, inventory):
        inventory["updated_at"] = self.get_timestamp()
        for i in range(len(self.data)):
            if self.data[i]["id"] == inventory_id:
                self.data[i] = inventory
                break

    def remove_inventory(self, inventory_id):
        for x in self.data:
            if x["id"] == inventory_id:
                self.data.remove(x)

    def load(self, is_debug):
        if is_debug:
            self.data = INVENTORIES
        else:
            f = open(self.data_path, "r")
            self.data = json.load(f)
            f.close()

    def save(self):
        f = open(self.data_path, "w")
        json.dump(self.data, f)
        f.close()