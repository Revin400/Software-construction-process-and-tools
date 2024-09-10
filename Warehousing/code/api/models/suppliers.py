import json

from models.base import Base

SUPPLIERS = [
    {
        "id": 1,
        "code": "SUP1001",
        "name": "Global Parts Ltd.",
        "address": "1000 Supplier Rd",
        "address_extra": "Building A",
        "city": "Industria",
        "zip_code": "99999",
        "province": "Manufacture State",
        "country": "Partland",
        "contact_name": "Alice Johnson",
        "contact_phone": "+123456789",
        "reference": "GP1001X",
        "created_at": "2024-03-01T08:00:00Z",
        "updated_at": "2024-03-05T09:00:00Z"
    },
    {
        "id": 2,
        "code": "SUP1002",
        "name": "Component Works",
        "address": "2000 Components Ave",
        "address_extra": "Suite 200",
        "city": "Gadget City",
        "zip_code": "88888",
        "province": "Gadget State",
        "country": "Widgetland",
        "contact_name": "Bob Smith",
        "contact_phone": "+987654321",
        "reference": "CW2002Y",
        "created_at": "2024-04-10T11:30:00Z",
        "updated_at": "2024-04-15T12:45:00Z"
    }
]

class Suppliers(Base):
    def __init__(self, root_path, is_debug=False):
        self.data_path = root_path + "suppliers.json"
        self.load(is_debug)

    def get_suppliers(self):
        return self.data

    def get_supplier(self, supplier_id):
        for x in self.data:
            if x["id"] == supplier_id:
                return x
        return None

    def add_supplier(self, supplier):
        supplier["created_at"] = self.get_timestamp()
        supplier["updated_at"] = self.get_timestamp()
        self.data.append(supplier)

    def update_supplier(self, supplier_id, supplier):
        supplier["updated_at"] = self.get_timestamp()
        for i in range(len(self.data)):
            if self.data[i]["id"] == supplier_id:
                self.data[i] = supplier
                break

    def remove_supplier(self, supplier_id):
        for x in self.data:
            if x["id"] == supplier_id:
                self.data.remove(x)

    def load(self, is_debug):
        if is_debug:
            self.data = SUPPLIERS
        else:
            f = open(self.data_path, "r")
            self.data = json.load(f)
            f.close()

    def save(self):
        f = open(self.data_path, "w")
        json.dump(self.data, f)
        f.close()