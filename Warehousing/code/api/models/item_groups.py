import json

from models.base import Base

ITEM_GROUPS = [
    {
        "id": 1,
        "name": "Electronics",
        "description": "All electronic devices and components.",
        "created_at": "2024-03-10T08:30:00Z",
        "updated_at": "2024-03-11T09:00:00Z"
    },
    {
        "id": 2,
        "name": "Hardware",
        "description": "Tools and hardware supplies.",
        "created_at": "2024-01-20T10:00:00Z",
        "updated_at": "2024-01-21T11:20:00Z"
    }
]

class ItemGroups(Base):
    def __init__(self, root_path, is_debug=False):
        self.data_path = root_path + "item_lines.json"
        self.load(is_debug)

    def get_item_groups(self):
        return self.data

    def get_item_group(self, item_group_id):
        for x in self.data:
            if x["id"] == item_group_id:
                return x
        return None

    def add_item_group(self, item_group):
        item_group["created_at"] = self.get_timestamp()
        item_group["updated_at"] = self.get_timestamp()
        self.data.append(item_group)

    def update_item_group(self, item_group_id, item_group):
        item_group["updated_at"] = self.get_timestamp()
        for i in range(len(self.data)):
            if self.data[i]["id"] == item_group_id:
                self.data[i] = item_group
                break

    def remove_item_group(self, item_group_id):
        for x in self.data:
            if x["id"] == item_group_id:
                self.data.remove(x)

    def load(self, is_debug):
        if is_debug:
            self.data = ITEM_GROUPS
        else:
            f = open(self.data_path, "r")
            self.data = json.load(f)
            f.close()

    def save(self):
        f = open(self.data_path, "w")
        json.dump(self.data, f)
        f.close()