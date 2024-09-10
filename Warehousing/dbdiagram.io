// Use DBML to define your database structure
// Docs: https://dbml.dbdiagram.io/docs

Table clients {
  id integer [primary key]
  name varchar
  address varchar
  city varchar
  zip_code varchar
  province varchar
  country varchar
  contact_name varchar
  contact_phone varchar
  contact_email varchar
  created_at timestamp
  updated_at timestamp
}

Table warehouses {
  id integer [primary key]
  code varchar [unique]
  name varchar
  address varchar
  city varchar
  zip_code varchar
  province varchar
  country varchar
  contact_name varchar
  contact_phone varchar
  contact_email varchar
  created_at timestamp
  updated_at timestamp
}

Table locations {
  id integer [primary key]
  warehouse_id integer
  code varchar [unique]
  name varchar
  created_at timestamp
  updated_at timestamp
}

Ref: locations.warehouse_id > warehouses.id

Table items {
  id integer [primary key]
  code varchar [unique]
  description text
  short_description tinytext
  upc_code integer
  model_number varchar
  commodity_code integer
  item_line integer
  item_group integer
  item_type integer
  unit_purchase_quantity integer
  unit_order_quantity integer
  pack_purchase_quantity integer
  pack_order_quantity integer
  supplier_id integer
  supplier_code varchar
  supplier_part_number integer
  created_at timestamp
  updated_at timestamp
}

Table item_lines {
  id integer [primary key]
  name varchar
  description text
  created_at timestamp
  updated_at timestamp
}

Table item_groups {
  id integer [primary key]
  name varchar
  description text
  created_at timestamp
  updated_at timestamp
}

Table item_types {
  id integer [primary key]
  name varchar
  description text
  created_at timestamp
  updated_at timestamp
}

Ref : items.item_group > item_groups.id
Ref : items.item_line > item_lines.id
Ref : items.item_type > item_types.id

Table inventory {
  item_id integer
  description text
  item_reference varchar
  location_id integer
  total_on_hand integer
  total_expected integer
  total_ordered integer
  total_allocated integer
  total_available integer
  created_at timestamp
  updated_at timestamp
}

Ref: inventory.item_id > items.id
Ref: inventory.location_id > locations.id

Table suppliers {
  id integer [primary key]
  code varchar [unique]
  name varchar
  address varchar
  address_extra varchar
  city varchar
  zip_code varchar
  province varchar
  country varchar
  contact_name varchar
  phonenumber varchar
  reference varchar
  created_at timestamp
  updated_at timestamp
}

Ref: items.supplier_id > suppliers.id

Table transfers {
  id integer [primary key]
  reference varchar
  transfer_from integer
  transfer_to integer
  transfer_status char [note: "S=Scheduled, P=Processed"]
  created_at timestamp
  updated_at timestamp
}

Table transfer_items {
  id integer [primary key]
  transfer_id integer
  item_id integer
  amount integer
}

Ref: transfers.transfer_from > locations.id
Ref: transfers.transfer_to > locations.id
Ref: transfer_items.transfer_id > transfers.id
Ref: transfer_items.item_id > items.id

Table orders {
  id integer [primary key]
  source_id integer
  order_date timestamp
  request_date timestamp
  reference varchar
  reference_extra varchar
  order_status char [note: "P=Pending, S=Shipped"]
  notes text
  shipping_notes text
  picking_notes text
  warehouse_id integer
  ship_to integer
  bill_to integer
  shipment_id integer
  total_amount decimal
  total_discount decimal
  total_tex decimal
  total_surcharge decimxal
  created_at timestamp
  updated_at timestamp
}

Table order_items {
  id integer [primary key]
  order_id integer
  item_id integer
  amount integer
}

Table shipments {
  id integer [primary key]
  reference varchar
  order_id integer
  source_id integer
  order_date timestamp
  request_date timestamp
  shipment_date timestamp
  shipment_type char [note: "I=Incoming, O=Outgoing"]
  shipment_status char [note: "S=Scheduled, T=Transit, D=Delivered"]
  notes text
  carrier_code varchar
  carrier_description text
  service_code varchar
  payment_type char [note: "M=Manual, A=Automated"]
  transfer_mode varchar
  total_package_count integer
  total_package_weight decimal
  created_at timestamp
  updated_at timestamp
}

Table shipment_items {
  id integer [primary key]
  shipment_id integer
  item_id integer
  amount integer
}

Ref: orders.warehouse_id > warehouses.id
Ref: orders.bill_to > clients.id
Ref: orders.ship_to > clients.id
Ref: orders.shipment_id > shipments.id
Ref: order_items.order_id > orders.id
Ref: order_items.item_id > items.id
Ref: shipment_items.shipment_id > shipments.id
Ref: shipment_items.item_id > items.id