using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddScoped<WarehouseService>();
builder.Services.AddScoped<ClientService>();


var app = builder.Build();

app.Urls.Add("http://localhost:5000");

app.MapControllers();


app.Run();