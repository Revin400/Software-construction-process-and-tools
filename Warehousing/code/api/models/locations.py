import json

from models.base import Base

LOCATIONS = [
    {
        "id": 1,
        "warehouse_id": 1,
        "code": "A.1.0",
        "name": "Row: A, Rack: 1, Shelf: 0",
        "created_at": "2024-03-20T10:00:00Z",
        "updated_at": "2024-03-20T10:00:00Z"
    },
    {
        "id": 2,
        "warehouse_id": 1,
        "code": "A.1.1",
        "name": "Row: A, Rack: 1, Shelf: 1",
        "created_at": "2024-03-20T10:00:00Z",
        "updated_at": "2024-03-20T10:00:00Z"
    },
    {
        "id": 3,
        "warehouse_id": 2,
        "code": "A.1.2",
        "name": "Row: A, Rack: 1, Shelf: 2",
        "created_at": "2024-03-20T10:00:00Z",
        "updated_at": "2024-03-20T10:00:00Z"
    },
    {
        "id": 4,
        "warehouse_id": 2,
        "code": "A.1.3",
        "name": "Row: A, Rack: 1, Shelf: 3",
        "created_at": "2024-03-20T10:00:00Z",
        "updated_at": "2024-03-20T10:00:00Z"
    }
]

class Locations(Base):
    def __init__(self, root_path, is_debug=False):
        self.data_path = root_path + "locations.json"
        self.load(is_debug)

    def get_locations(self):
        return self.data

    def get_location(self, location_id):
        for x in self.data:
            if x["id"] == location_id:
                return x
        return None

    def get_locations_in_warehouse(self, warehouse_id):
        result = []
        for x in self.data:
            if x["warehouse_id"] == warehouse_id:
                result.append(x)
        return result

    def add_location(self, location):
        location["created_at"] = self.get_timestamp()
        location["updated_at"] = self.get_timestamp()
        self.data.append(location)

    def update_location(self, location_id, location):
        location["updated_at"] = self.get_timestamp()
        for i in range(len(self.data)):
            if self.data[i]["id"] == location_id:
                self.data[i] = location
                break

    def remove_location(self, location_id):
        for x in self.data:
            if x["id"] == location_id:
                self.data.remove(x)

    def load(self, is_debug):
        if is_debug:
            self.data = LOCATIONS
        else:
            f = open(self.data_path, "r")
            self.data = json.load(f)
            f.close()

    def save(self):
        f = open(self.data_path, "w")
        json.dump(self.data, f)
        f.close()