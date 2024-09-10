import json

from models.base import Base

TRANSFERS = [
    {
        "id": 1,
        "reference": "TRANS001",
        "transfer_from": 1,
        "transfer_to": 2,
        "transfer_status": "Processed",
        "created_at": "2024-05-01T14:00:00Z",
        "updated_at": "2024-05-02T15:00:00Z",
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
        "reference": "TRANS002",
        "transfer_from": 2,
        "transfer_to": 1,
        "transfer_status": "Scheduled",
        "created_at": "2024-05-03T10:30:00Z",
        "updated_at": "2024-05-04T11:30:00Z",
        "items": [
            {
                "item_id": 1,
                "amount": 20
            },
            {
                "item_id": 2,
                "amount": 40
            }
        ]
    }
]

class Transfers(Base):
    def __init__(self, root_path, is_debug=False):
        self.data_path = root_path + "transfers.json"
        self.load(is_debug)

    def get_transfers(self):
        return self.data

    def get_transfer(self, transfer_id):
        for x in self.data:
            if x["id"] == transfer_id:
                return x
        return None

    def get_items_in_transfer(self, transfer_id):
        for x in self.data:
            if x["id"] == transfer_id:
                return x["items"]
        return None

    def add_transfer(self, transfer):
        transfer["transfer_status"] = "Scheduled"
        transfer["created_at"] = self.get_timestamp()
        transfer["updated_at"] = self.get_timestamp()
        self.data.append(transfer)

    def update_transfer(self, transfer_id, transfer):
        transfer["updated_at"] = self.get_timestamp()
        for i in range(len(self.data)):
            if self.data[i]["id"] == transfer_id:
                self.data[i] = transfer
                break

    def remove_transfer(self, transfer_id):
        for x in self.data:
            if x["id"] == transfer_id:
                self.data.remove(x)

    def load(self, is_debug):
        if is_debug:
            self.data = TRANSFERS
        else:
            f = open(self.data_path, "r")
            self.data = json.load(f)
            f.close()

    def save(self):
        f = open(self.data_path, "w")
        json.dump(self.data, f)
        f.close()