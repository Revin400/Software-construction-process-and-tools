# CargoHUB API

This is the CargoHUB API guide. Its description is inspired by the realworld Sphere Warehouse Management System (WMS), see:

Sphere website: https://spherewms.com

Sphere api integration: https://spherewms.com/integrations/api

Shere api description: https://api.spherewms.com/v2/wms.

## Accessing the API

The following is the url endpoint for the api, version 1:

`https://cargohub.com/api/v1`

You need an API key to access the API. API keys and their corresponding endpoint access must be manually added to the Authentication Provider (`auth_provider.py`), for example:

```json
{
    "api_key": "f6g7h8i9j0",
    "app": "CargoHUB Dashboard 2",
    "endpoint_access": {
        "full": false,
        "warehouses": {
            "full": false,
            "get": true,
            "post": false,
            "put": false,
            "delete": false
        },
        "locations":  {
            "full": false,
            "get": true,
            "post": false,
            "put": false,
            "delete": false
        },
        "transfers":  {
            "full": false,
            "get": true,
            "post": false,
            "put": false,
            "delete": false
        },
        "items":  {
            "full": false,
            "get": true,
            "post": false,
            "put": false,
            "delete": false
        },
        "item_lines":  {
            "full": false,
            "get": true,
            "post": false,
            "put": false,
            "delete": false
        },
        "item_groups":  {
            "full": false,
            "get": true,
            "post": false,
            "put": false,
            "delete": false
        },
        "item_types":  {
            "full": false,
            "get": true,
            "post": false,
            "put": false,
            "delete": false
        },
        "suppliers":  {
            "full": false,
            "get": true,
            "post": false,
            "put": false,
            "delete": false
        },
        "orders":  {
            "full": false,
            "get": true,
            "post": false,
            "put": false,
            "delete": false
        },
        "clients":  {
            "full": false,
            "get": true,
            "post": false,
            "put": false,
            "delete": false
        },
        "shipments":  {
            "full": false,
            "get": true,
            "post": false,
            "put": false,
            "delete": false
        }
    }
}
```

#### Request Header

`API_KEY: {api_key}`

The API returns

- `401 Unauthorized` when the key does not match or cannot be found
- `403 Forbidden` when the key does not have access to a resource

#### When everything is okay

The api returns

- `200 OK` when a resource is fetched, changed or deleted succesfully
- `201 Created` when a resource is added succesfully

#### Upon failure

When an error occurs the api returns `500 Internal Server Error`.

## Endpoints

Below you find a complete description of the endpoints.

### Warehouses

Endpoint for accessing and manipulating warehouse objects.

#### Warehouse object fields

```json
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
}
```

#### List all warehouses

`GET /api/v1/warehouses`

##### Request body

`None`

##### Response body

```json
[
    {
        warehouse_fields_1
    },
    {
        warehouse_fields_2
    },
    ...
]
```

#### Get a specific warehouse

`GET /api/v1/warehouses/{warehouse_id}`

##### Request body

`None`

##### Response body

```json
{
    warehouse_fields
}
```

#### Get the storage locations of a specific warehouse

`GET /api/v1/warehouses/{warehouse_id}/locations`

##### Request body

`None`

##### Response body

```json
{
    location_fields
}
```

#### Create a new warehouse

`POST /api/v1/warehouses`

#### Request body

```json
{
    warehouse_fields
}
```

##### Response body

`None`

#### Update a specific warehouse

`PUT /api/v1/warehouses/{warehouse_id}`

#### Request body

```json
{
    warehouse_fields
}
```

##### Response body

`None`

#### Delete a warehouse

`DELETE /api/v1/warehouses/{warehouse_id}`

##### Request body

`None`

##### Response body

`None`

### Locations

Endpoint for accessing and manipulating location objects.

#### Location object fields

```json
{
    "id": 1,
    "warehouse_id": 1,
    "code": "A.1.0",
    "name": "Row: A, Rack: 1, Shelf: 0",
    "created_at": "2024-01-01T12:00:00Z",
    "updated_at": "2024-01-01T12:00:00Z"
}
```

#### List all locations

`GET /api/v1/locations`

##### Request body

`None`

##### Response body

```json
[
    {
        location_fields_1
    },
    {
        location_fields_2
    },
    ...
]
```

#### Get a specific location

`GET /api/v1/locations/{location_id}`

##### Request body

`None`

##### Response body

```json
{
    location_fields
}
```

#### Create a new location

`POST /api/v1/locations`

#### Request body

```json
{
    location_fields
}
```

##### Response body

`None`

#### Update a specific location

`PUT /api/v1/locations/{location_id}`

#### Request body

```json
{
    location_fields
}
```

##### Response body

`None`

#### Delete a location

`DELETE /api/v1/locations/{location_id}`

##### Request body

`None`

##### Response body

`None`

### Transfers

Endpoint for accessing and manipulating transfer objects.

#### Transfer object fields

```json
{
    "id": 1,
    "reference": "TRANS001",
    "transfer_from": 1,
    "transfer_to": 2,
    "transfer_status": "Scheduled | Processed",
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
}
```

#### List all transfers

`GET /api/v1/transfers`

##### Request body

`None`

##### Response body

```json
[
    {
        transfer_fields_1
    },
    {
        transfer_fields_2
    },
    ...
]
```

#### Get a specific transfer

`GET /api/v1/transfers/{transfer_id}`

##### Request body

`None`

##### Response body

```json
{
    transfer_fields
}
```

#### Get the items in a specific transfer

`GET /api/v1/transfers/{transfer_id}/items`

##### Request body

`None`

##### Response body

```json
[
    {
        "item_id": 1,
        "amount": 50
    },
    {
        "item_id": 2,
        "amount": 30
    }
    ...
]
```

#### Create a new transfer

`POST /api/v1/transfers`

#### Request body

```json
{
    transfer_fields
}
```

##### Response body

`None`

#### Update a scheduled transfer

`PUT /api/v1/transfers/{transfer_id}`

#### Request body

```json
{
    transfer_fields
}
```

##### Response body

`None`

#### Commit a scheduled transfer and update the corresponding inventory

`PUT /api/v1/transfers/{transfer_id}/commit`

#### Request body

`None`

##### Response body

`None`

#### Delete a transfer

`DELETE /api/v1/transfers/{transfer_id}`

##### Request body

`None`

##### Response body

`None`

### Items

Endpoint for accessing and manipulating item objects.

#### Item object fields

```json
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
    "created_at": "2024-01-01T12:00:00Z",
    "updated_at": "2024-01-01T12:00:00Z"
}
```

#### List all items

`GET /api/v1/items`

##### Request body

`None`

##### Response body

```json
[
    {
        item_fields_1
    },
    {
        item_fields_2
    },
    ...
]
```

#### Get a specific item

`GET /api/v1/items/{item_id}`

##### Request body

`None`

##### Response body

```json
{
    item_fields
}
```

#### Get the inventory of a specific item

`GET /api/v1/items/{item_id}/inventory`

##### Request body

`None`

##### Response body

```json
[
    {
        inventory_fields_1
    },
    {
        inventory_fields_1
    },
    ...
]
```

#### Get the inventory totals of a specific item

`GET /api/v1/items/{item_id}/inventory/totals`

##### Request body

`None`

##### Response body

```json
{
    "total_expected": 200,
    "total_ordered": 50,
    "total_allocated": 100,
    "total_available": 50
}
```

#### Create a new item

`POST /api/v1/items`

#### Request body

```json
{
    item_fields
}
```

##### Response body

`None`

#### Update a specific item

`PUT /api/v1/items/{item_id}`

#### Request body

```json
{
    item_fields
}
```

##### Response body

`None`

#### Delete a item

`DELETE /api/v1/items/{item_id}`

##### Request body

`None`

##### Response body

`None`

### Item Lines

Endpoint for accessing and manipulating item line objects.

#### Item Line object fields

```json
{
    "id": 1,
    "name": "Construction Materials",
    "description": "A comprehensive range of building and construction materials including cement, bricks, and tiles.",
    "created_at": "2024-03-05T09:00:00Z",
    "updated_at": "2024-03-06T10:00:00Z"
}
```

#### List all item lines

`GET /api/v1/item_lines`

##### Request body

`None`

##### Response body

```json
[
    {
        item_line_fields_1
    },
    {
        item_line_fields_2
    },
    ...
]
```

#### Get a specific item line

`GET /api/v1/item_lines/{item_line_id}`

##### Request body

`None`

##### Response body

```json
{
    item_line_fields
}
```

#### Get all items for this specific item line

`GET /api/v1/item_lines/{item_line_id}/items`

##### Request body

`None`

##### Response body

```json
[
    "item_id": 1,
    "item_id": 2,
    ...
]
```

#### Create a new item

`POST /api/v1/item_lines`

#### Request body

```json
{
    item_line_fields
}
```

##### Response body

`None`

#### Update a specific item

`PUT /api/v1/item_lines/{item_line_id}`

#### Request body

```json
{
    item_line_fields
}
```

##### Response body

`None`

#### Delete a item

`DELETE /api/v1/item_lines/{item_line_id}`

##### Request body

`None`

##### Response body

`None`

### Item Groups

Endpoint for accessing and manipulating item group objects.

#### Item Group object fields

```json
{
    "id": 1,
    "name": "Electronics",
    "description": "All electronic devices and components.",
    "created_at": "2024-03-10T08:30:00Z",
    "updated_at": "2024-03-11T09:00:00Z"
}
```

#### List all item groups

`GET /api/v1/item_groups`

##### Request body

`None`

##### Response body

```json
[
    {
        item_group_fields_1
    },
    {
        item_group_fields_2
    },
    ...
]
```

#### Get a specific item group

`GET /api/v1/item_groups/{item_group_id}`

##### Request body

`None`

##### Response body

```json
{
    item_group_fields
}
```

#### Get all items for this specific item group

`GET /api/v1/item_groups/{item_group_id}/items`

##### Request body

`None`

##### Response body

```json
[
    "item_id": 1,
    "item_id": 2,
    ...
]
```

#### Create a new item

`POST /api/v1/item_groups`

#### Request body

```json
{
    item_group_fields
}
```

##### Response body

`None`

#### Update a specific item

`PUT /api/v1/item_groups/{item_group_id}`

#### Request body

```json
{
    item_group_fields
}
```

##### Response body

`None`

#### Delete a item

`DELETE /api/v1/item_groups/{item_group_id}`

##### Request body

`None`

##### Response body

`None`

### Item Types

Endpoint for accessing and manipulating item type objects.

#### Item Type object fields

```json
{
    "id": 1,
    "name": "Power Tools",
    "description": "Tools powered by electric motors or batteries, including drills, saws, and sanders.",
    "created_at": "2024-04-10T08:30:00Z",
    "updated_at": "2024-04-11T09:00:00Z"
}
```

#### List all item types

`GET /api/v1/item_types`

##### Request body

`None`

##### Response body

```json
[
    {
        item_type_fields_1
    },
    {
        item_type_fields_2
    },
    ...
]
```

#### Get a specific item type

`GET /api/v1/item_types/{item_type_id}`

##### Request body

`None`

##### Response body

```json
{
    item_type_fields
}
```

#### Get all items for this specific item type

`GET /api/v1/item_types/{item_type_id}/items`

##### Request body

`None`

##### Response body

```json
[
    "item_id": 1,
    "item_id": 2,
    ...
]
```

#### Create a new item

`POST /api/v1/item_types`

#### Request body

```json
{
    item_type_fields
}
```

##### Response body

`None`

#### Update a specific item

`PUT /api/v1/item_types/{item_type_id}`

#### Request body

```json
{
    item_type_fields
}
```

##### Response body

`None`

#### Delete a item

`DELETE /api/v1/item_types/{item_type_id}`

##### Request body

`None`

##### Response body

`None`

### Inventories

Endpoint for accessing and manipulating inventory objects.

#### Inventory object fields

```json
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
}
```

#### List all inventories

`GET /api/v1/inventories`

##### Request body

`None`

##### Response body

```json
[
    {
        inventory_fields_1
    },
    {
        inventory_fields_2
    },
    ...
]
```

#### Get a specific inventory

`GET /api/v1/inventories/{inventory_id}`

##### Request body

`None`

##### Response body

```json
[
    {
        inventory_fields_1
    },
    {
        inventory_fields_2
    },
    ...
]
```

#### Create a new inventory

`POST /api/v1/inventories`

#### Request body

```json
{
    inventory_fields
}
```

##### Response body

`None`

#### Update a specific inventory

`PUT /api/v1/inventories/{inventory_id}`

#### Request body

```json
{
    "amount": 12
}
```

##### Response body

`None`

#### Delete an inventory

`DELETE /api/v1/inventories/{inventory_id}`

##### Request body

`None`

##### Response body

`None`

### Suppliers

Endpoint for accessing and manipulating supplier objects.

#### Supplier object fields

```json
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
}
```

#### List all suppliers

`GET /api/v1/suppliers`

##### Request body

`None`

##### Response body

```json
[
    {
        supplier_fields_1
    },
    {
        supplier_fields_2
    },
    ...
]
```

#### Get a specific supplier

`GET /api/v1/suppliers/{supplier_id}`

##### Request body

`None`

##### Response body

```json
{
    supplier_fields
}
```

#### Get the items of a specific supplier

`GET /api/v1/suppliers/{supplier_id}/items`

##### Request body

`None`

##### Response body

```json
[
    {
        item_fields_1
    },
    {
        item_fields_2
    },
    ...
]
```

#### Create a new supplier

`POST /api/v1/suppliers`

#### Request body

```json
{
    supplier_fields
}
```

##### Response body

`None`

#### Update a specific supplier

`PUT /api/v1/suppliers/{supplier_id}`

#### Request body

```json
{
    supplier_fields
}
```

##### Response body

`None`

#### Delete a supplier

`DELETE /api/v1/suppliers/{supplier_id}`

##### Request body

`None`

##### Response body

`None`

### Orders

Endpoint for accessing and manipulating order objects.

#### Order object fields

```json
{
    "id": 1,
    "source_id": 101,
    "order_date": "2024-05-01T13:45:00Z",
    "request_date": "2024-05-05T00:00:00Z",
    "reference": "ORD001",
    "reference_extra": "First bulk order",
    "order_status": "Scheduled | Packed | Delivered",
    "notes": "Please ensure timely delivery.",
    "shipping_notes": "Handle with care.",
    "picking_notes": "Verify items before dispatch.",
    "warehouse_id": 3,
    "ship_to": 10,
    "bill_to": 10,
    "shipment_id": 1,
    "total_amount": 5000.00,
    "total_discount": 100.00,
    "total_tax": 475.00,
    "total_surcharge": 30.00,
    "created_at": "2024-05-01T13:45:00Z",
    "updated_at": "2024-05-02T09:30:00Z",
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
}
```

#### List all orders

`GET /api/v1/orders`

##### Request body

`None`

##### Response body

```json
[
    {
        order_fields_1
    },
    {
        order_fields_2
    },
    ...
]
```

#### Get a specific order

`GET /api/v1/orders/{order_id}`

##### Request body

`None`

##### Response body

```json
{
    order_fields
}
```

#### Get the items in a specific order

`GET /api/v1/orders/{order_id}/items`

##### Request body

`None`

##### Response body

```json
[
    {
        "item_id": 1,
        "amount": 50
    },
    {
        "item_id": 2,
        "amount": 30
    },
    ...
]
```

#### Create a new order

`POST /api/v1/orders`

#### Request body

```json
{
    order_fields
}
```

##### Response body

`None`

#### Update a specific order

`PUT /api/v1/orders/{order_id}`

#### Request body

```json
{
    order_fields
}
```

##### Response body

`None`

#### Update the items of a specific order

`PUT /api/v1/orders/{order_id}/items`

#### Request body

```json
[
    {
        "item_id": 1,
        "amount": 50
    },
    {
        "item_id": 2,
        "amount": 30
    },
    ...
]
```

##### Response body

`None`

#### Delete a order

`DELETE /api/v1/orders/{order_id}`

##### Request body

`None`

##### Response body

`None`

### Clients

Endpoint for accessing and manipulating client objects.

#### Client object fields

```json
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
}
```

#### List all clients

`GET /api/v1/clients`

##### Request body

`None`

##### Response body

```json
[
    {
        client_fields_1
    },
    {
        client_fields_2
    },
    ...
]
```

#### Get a specific client

`GET /api/v1/clients/{client_id}`

##### Request body

`None`

##### Response body

```json
{
    client_fields
}
```

#### Get the orders of a specific client

`GET /api/v1/clients/{client_id}/orders`

##### Request body

`None`

##### Response body

```json
[
    {
        order_fields_1
    },
    {
        order_fields_2
    },
    ...
]
```

#### Create a new client

`POST /api/v1/clients`

#### Request body

```json
{
    client_fields
}
```

##### Response body

`None`

#### Update a specific client

`PUT /api/v1/clients/{client_id}`

#### Request body

```json
{
    client_fields
}
```

##### Response body

`None`

#### Delete a client

`DELETE /api/v1/clients/{client_id}`

##### Request body

`None`

##### Response body

`None`

### Shipments

Endpoint for accessing and manipulating shipment objects.

#### Shipment object fields

```json
{
    "id": 1,
    "order_id": 1,
    "source_id": 501,
    "order_date": "2024-05-01",
    "request_date": "2024-05-03",
    "shipment_date": "2024-05-05",
    "shipment_type": "Incoming | Outgoing",
    "shipment_status": "Scheduled | Transit | Delivered",
    "notes": "Express shipment for urgent requirement.",
    "carrier_code": "UPS",
    "carrier_description": "United Parcel Service",
    "service_code": "NextDay",
    "payment_type": "Cash | Check | Bank | Bitcoin",
    "transfer_mode": "Freight | Ship | Air",
    "total_package_count": 10,
    "total_package_weight": 200.5,
    "created_at": "2024-05-01T12:00:00Z",
    "updated_at": "2024-05-02T14:00:00Z",
    "items": [
        {
            "item_id": 1,
            "amount": 5
        },
        {
            "item_id": 2,
            "amount": 10
        }
    ]
}
```

#### List all shipments

`GET /api/v1/shipments`

##### Request body

`None`

##### Response body

```json
[
    {
        shipment_fields_1
    },
    {
        shipment_fields_2
    },
    ...
]
```

#### Get a specific shipment

`GET /api/v1/shipments/{shipment_id}`

##### Request body

`None`

##### Response body

```json
{
    shipment_fields
}
```

#### Get the order ids in a specific shipment

`GET /api/v1/shipments/{shipment_id}/orders`

##### Request body

`None`

##### Response body

```json
[
    "order_id": 1,
    "order_id": 2,
    ...
]
```

#### Get the shipped items in a specific shipment

`GET /api/v1/shipments/{shipment_id}/items`

##### Request body

`None`

##### Response body

```json
[
    {
        "item_id": 1,
        "amount": 5
    },
    {
        "item_id": 2,
        "amount": 10
    },
    ...
]
```

#### Create a new shipment

`POST /api/v1/shipments`

#### Request body

```json
{
    shipment_fields
}
```

##### Response body

`None`

#### Update a specific shipment

`PUT /api/v1/shipments/{shipment_id}`

#### Request body

```json
{
    shipment_fields
}
```

##### Response body

`None`

#### Update the orders of a specific shipment

`PUT /api/v1/shipments/{shipment_id}/orders`

#### Request body

```json
[
    "order_id": 1,
    "order_id": 2,
]
```

##### Response body

`None`

#### Update the items of a specific shipment

`PUT /api/v1/shipments/{shipment_id}/items`

#### Request body

```json
[
    {
        "item_id": 1,
        "amount": 5
    },
    {
        "item_id": 2,
        "amount": 10
    },
    ...
]
```

##### Response body

`None`

#### Delete a shipment

`DELETE /api/v1/shipments/{shipment_id}`

##### Request body

`None`

##### Response body

`None`