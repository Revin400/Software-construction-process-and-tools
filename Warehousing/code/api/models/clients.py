import json

from models.base import Base

CLIENTS = [
    {
        "id": 1,
        "name": "Alpha Industries",
        "address": "1234 Industrial Way",
        "city": "Techville",
        "zip_code": "90909",
        "province": "TechState",
        "country": "Techland",
        "contact_name": "John Doe",
        "contact_phone": "123-456-7890",
        "contact_email": "johndoe@alphaindustries.com",
        "created_at": "2024-01-15T09:30:00Z",
        "updated_at": "2024-01-20T10:30:00Z"
    },
    {
        "id": 2,
        "name": "Beta Corporation",
        "address": "4321 Corporate Blvd",
        "city": "MarketTown",
        "zip_code": "80808",
        "province": "BizState",
        "country": "Bizland",
        "contact_name": "Jane Smith",
        "contact_phone": "321-654-0987",
        "contact_email": "janesmith@betacorp.com",
        "created_at": "2024-02-11T14:20:00Z",
        "updated_at": "2024-02-15T15:25:00Z"
    }
]

class Clients(Base):
    def __init__(self, root_path, is_debug=False):
        self.data_path = root_path + "clients.json"
        self.load(is_debug)

    def get_clients(self):
        return self.data

    def get_client(self, client_id):
        for x in self.data:
            if x["id"] == client_id:
                return x
        return None

    def add_client(self, client):
        client["created_at"] = self.get_timestamp()
        client["updated_at"] = self.get_timestamp()
        self.data.append(client)

    def update_client(self, client_id, client):
        client["updated_at"] = self.get_timestamp()
        for i in range(len(self.data)):
            if self.data[i]["id"] == client_id:
                self.data[i] = client
                break

    def remove_client(self, client_id):
        for x in self.data:
            if x["id"] == client_id:
                self.data.remove(x)

    def load(self, is_debug):
        if is_debug:
            self.data = CLIENTS
        else:
            f = open(self.data_path, "r")
            self.data = json.load(f)
            f.close()

    def save(self):
        f = open(self.data_path, "w")
        json.dump(self.data, f)
        f.close()