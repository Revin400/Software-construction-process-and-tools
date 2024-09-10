import json

from models.base import Base

ITEM_LINES = [
    {
        "id": 1,
        "name": "Construction Materials",
        "description": "A comprehensive range of building and construction materials including cement, bricks, and tiles.",
        "created_at": "2024-03-05T09:00:00Z",
        "updated_at": "2024-03-06T10:00:00Z"
    },
    {
        "id": 2,
        "name": "Automotive Parts",
        "description": "Parts and accessories for various types of vehicles, focusing on both replacement parts and performance upgrades.",
        "created_at": "2024-05-01T15:00:00Z",
        "updated_at": "2024-05-02T16:30:00Z"
    }
]

class ItemLines(Base):
    def __init__(self, root_path, is_debug=False):
        self.data_path = root_path + "item_lines.json"
        self.load(is_debug)

    def get_item_lines(self):
        return self.data

    def get_item_line(self, item_line_id):
        for x in self.data:
            if x["id"] == item_line_id:
                return x
        return None

    def add_item_line(self, item_line):
        item_line["created_at"] = self.get_timestamp()
        item_line["updated_at"] = self.get_timestamp()
        self.data.append(item_line)

    def update_item_line(self, item_line_id, item_line):
        item_line["updated_at"] = self.get_timestamp()
        for i in range(len(self.data)):
            if self.data[i]["id"] == item_line_id:
                self.data[i] = item_line
                break

    def remove_item_line(self, item_line_id):
        for x in self.data:
            if x["id"] == item_line_id:
                self.data.remove(x)

    def load(self, is_debug):
        if is_debug:
            self.data = ITEM_LINES
        else:
            f = open(self.data_path, "r")
            self.data = json.load(f)
            f.close()

    def save(self):
        f = open(self.data_path, "w")
        json.dump(self.data, f)
        f.close()