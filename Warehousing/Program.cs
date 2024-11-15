using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<ClientDbContext>();
builder.Services.AddControllers();
builder.Services.AddScoped<WarehouseService>();
builder.Services.AddScoped<ClientService>();
builder.Services.AddScoped<LocationService>();


var app = builder.Build();

app.Urls.Add("http://localhost:5000");

app.MapControllers();


app.Run();

Console.WriteLine("Starting up the application...");