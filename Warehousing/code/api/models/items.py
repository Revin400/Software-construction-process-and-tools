import json

from models.base import Base

ITEMS = [
    {
        "id": 1,
        "code": "ABC123",
        "description": "High-performance laptop",
        "short_description": "Laptop",
        "upc_code": "123456789012",
        "model_number": "LT-2024",
        "commodity_code": "IT-EQUIP",
        "item_line_id": 1,
        "item_group_id": 1,
        "item_type_id": 1,
        "unit_purchase_quantity": 1,
        "unit_order_quantity": 1,
        "pack_order_quantity": 1,
        "supplier_id": 1,
        "supplier_code": "SUP001",
        "supplier_part_number": "LT-2024-ABC",
        "created_at": "2024-03-20T10:00:00Z",
        "updated_at": "2024-03-20T10:00:00Z"
    },
    {
        "id": 2,
        "code": "XYZ789",
        "description": "Wireless Bluetooth headphones",
        "short_description": "Headphones",
        "upc_code": "987654321098",
        "model_number": "HP-2024",
        "commodity_code": "AUDIO-EQUIP",
        "item_line_id": 2,
        "item_group_id": 2,
        "item_type_id": 2,
        "unit_purchase_quantity": 1,
        "unit_order_quantity": 1,
        "pack_order_quantity": 1,
        "supplier_id": 2,
        "supplier_code": "SUP002",
        "supplier_part_number": "HP-2024-XYZ",
        "created_at": "2024-03-20T10:00:00Z",
        "updated_at": "2024-03-20T10:00:00Z"
    }
]

class Items(Base):
    def __init__(self, root_path, is_debug=False):
        self.data_path = root_path + "items.json"
        self.load(is_debug)

    def get_items(self):
        return self.data

    def get_item(self, item_id):
        for x in self.data:
            if x["id"] == item_id:
                return x
        return None

    def get_items_for_item_line(self, item_line_id):
        result = []
        for x in self.data:
            if x["item_line_id"] == item_line_id:
                result.append(x["id"])
        return result

    def get_items_for_item_group(self, item_group_id):
        result = []
        for x in self.data:
            if x["item_group_id"] == item_group_id:
                result.append(x["id"])
        return result

    def get_items_for_item_type(self, item_type_id):
        result = []
        for x in self.data:
            if x["item_type_id"] == item_type_id:
                result.append(x["id"])
        return result

    def get_items_for_supplier(self, supplier_id):
        result = []
        for x in self.data:
            if x["supplier_id"] == supplier_id:
                result.append(x)
        return result

    def add_item(self, item):
        item["created_at"] = self.get_timestamp()
        item["updated_at"] = self.get_timestamp()
        self.data.append(item)

    def update_item(self, item_id, item):
        item["updated_at"] = self.get_timestamp()
        for i in range(len(self.data)):
            if self.data[i]["id"] == item_id:
                self.data[i] = item
                break

    def remove_item(self, item_id):
        for x in self.data:
            if x["id"] == item_id:
                self.data.remove(x)

    def load(self, is_debug):
        if is_debug:
            self.data = ITEMS
        else:
            f = open(self.data_path, "r")
            self.data = json.load(f)
            f.close()

    def save(self):
        f = open(self.data_path, "w")
        json.dump(self.data, f)
        f.close()