using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<WarehousingContext>();

builder.Services.AddControllers();
builder.Services.AddScoped<Warehousing.DataServices_v1.ClientService>();
builder.Services.AddScoped<Warehousing.DataServices_v2.ClientService>();

builder.Services.AddScoped<Warehousing.DataServices_v1.WarehouseService>();
builder.Services.AddScoped<Warehousing.DataServices_v2.WarehouseService>();

builder.Services.AddScoped<Warehousing.DataServices_v1.LocationService>();
builder.Services.AddScoped<Warehousing.DataServices_v2.LocationService>();

builder.Services.AddScoped<Warehousing.DataServices_v1.ItemService>();
builder.Services.AddScoped<Warehousing.DataServices_v2.ItemService>();

builder.Services.AddScoped<Warehousing.DataServices_v1.ItemGroupService>();
builder.Services.AddScoped<Warehousing.DataServices_v2.ItemGroupService>();

builder.Services.AddScoped<Warehousing.DataServices_v1.ItemTypeService>();
builder.Services.AddScoped<Warehousing.DataServices_v2.ItemTypeService>();

builder.Services.AddScoped<Warehousing.DataServices_v1.ItemLineService>();
builder.Services.AddScoped<Warehousing.DataServices_v2.ItemLineService>();

builder.Services.AddScoped<Warehousing.DataServices_v1.InventoryService>();
builder.Services.AddScoped<Warehousing.DataServices_v2.InventoryService>();

builder.Services.AddScoped<Warehousing.DataServices_v1.TransferService>();
builder.Services.AddScoped<Warehousing.DataServices_v2.TransferService>();

builder.Services.AddScoped<Warehousing.DataServices_v1.OrderService>();
builder.Services.AddScoped<Warehousing.DataServices_v2.OrderService>();

builder.Services.AddScoped<Warehousing.DataServices_v1.ShipmentService>();
builder.Services.AddScoped<Warehousing.DataServices_v2.ShipmentService>();

builder.Services.AddScoped<Warehousing.DataServices_v1.SupplierService>();
builder.Services.AddScoped<Warehousing.DataServices_v2.SupplierService>();



var app = builder.Build();

app.UseMiddleware<ApiKeyMiddleware>();

app.Urls.Add("http://localhost:5000");

app.MapControllers();

app.Run();

Console.WriteLine("Starting up the application...");