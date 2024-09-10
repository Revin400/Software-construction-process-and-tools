import json

from models.base import Base

WAREHOUSES = [
    {
        "id": 1,
        "code": "WH001",
        "name": "Central Storage",
        "address": "100 Main St",
        "city": "Centerville",
        "zip_code": "90001",
        "province": "Central State",
        "country": "Centralland",
        "contact_name": "Tom Harris",
        "contact_phone": "123-456-7890",
        "contact_email": "tomh@centralstorage.com",
        "created_at": "2024-01-01T12:00:00Z",
        "updated_at": "2024-01-02T12:00:00Z"
    },
    {
        "id": 2,
        "code": "WH002",
        "name": "East Distribution Center",
        "address": "200 East Blvd",
        "city": "Eastville",
        "zip_code": "90002",
        "province": "Eastern State",
        "country": "Eastland",
        "contact_name": "Sara Miller",
        "contact_phone": "987-654-3210",
        "contact_email": "saram@eastdistrib.com",
        "created_at": "2024-02-01T15:00:00Z",
        "updated_at": "2024-02-02T16:00:00Z"
    }
]

class Warehouses(Base):
    def __init__(self, root_path, is_debug=False):
        self.data_path = root_path + "warehouses.json"
        self.load(is_debug)

    def get_warehouses(self):
        return self.data

    def get_warehouse(self, warehouse_id):
        for x in self.data:
            if x["id"] == warehouse_id:
                return x
        return None

    def add_warehouse(self, warehouse):
        warehouse["created_at"] = self.get_timestamp()
        warehouse["updated_at"] = self.get_timestamp()
        self.data.append(warehouse)

    def update_warehouse(self, warehouse_id, warehouse):
        warehouse["updated_at"] = self.get_timestamp()
        for i in range(len(self.data)):
            if self.data[i]["id"] == warehouse_id:
                self.data[i] = warehouse
                break

    def remove_warehouse(self, warehouse_id):
        for x in self.data:
            if x["id"] == warehouse_id:
                self.data.remove(x)

    def load(self, is_debug):
        if is_debug:
            self.data = WAREHOUSES
        else:
            f = open(self.data_path, "r")
            self.data = json.load(f)
            f.close()

    def save(self):
        f = open(self.data_path, "w")
        json.dump(self.data, f)
        f.close()