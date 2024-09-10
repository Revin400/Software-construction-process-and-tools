import json

from models.base import Base

ITEM_TYPES = [
    {
        "id": 1,
        "name": "Power Tools",
        "description": "Tools powered by electric motors or batteries, including drills, saws, and sanders.",
        "created_at": "2024-04-10T08:30:00Z",
        "updated_at": "2024-04-11T09:00:00Z"
    },
    {
        "id": 2,
        "name": "Hand Tools",
        "description": "Non-power tools that require manual effort to operate, such as screwdrivers, hammers, and wrenches.",
        "created_at": "2024-02-20T10:00:00Z",
        "updated_at": "2024-02-21T11:20:00Z"
    }
]

class ItemTypes(Base):
    def __init__(self, root_path, is_debug=False):
        self.data_path = root_path + "item_types.json"
        self.load(is_debug)

    def get_item_types(self):
        return self.data

    def get_item_type(self, item_type_id):
        for x in self.data:
            if x["id"] == item_type_id:
                return x
        return None

    def add_item_type(self, item_type):
        item_type["created_at"] = self.get_timestamp()
        item_type["updated_at"] = self.get_timestamp()
        self.data.append(item_type)

    def update_item_type(self, item_type_id, item_type):
        item_type["updated_at"] = self.get_timestamp()
        for i in range(len(self.data)):
            if self.data[i]["id"] == item_type_id:
                self.data[i] = item_type
                break

    def remove_item_type(self, item_type_id):
        for x in self.data:
            if x["id"] == item_type_id:
                self.data.remove(x)

    def load(self, is_debug):
        if is_debug:
            self.data = ITEM_TYPES
        else:
            f = open(self.data_path, "r")
            self.data = json.load(f)
            f.close()

    def save(self):
        f = open(self.data_path, "w")
        json.dump(self.data, f)
        f.close()