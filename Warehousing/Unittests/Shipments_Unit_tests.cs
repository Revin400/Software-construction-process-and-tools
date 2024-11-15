using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Xunit;

public class Shipments_Unit_Tests
{
    private readonly HttpClient _client;

    public Shipments_Unit_Tests()
    {
        _client = new HttpClient { BaseAddress = new Uri("http://localhost:5000/api/") };
    }

    [Fact]
    public async Task GetAllShipments_ShouldReturnSuccess()
    {
        var response = await _client.GetAsync("shipment");
        response.EnsureSuccessStatusCode();

        var shipments = await response.Content.ReadAsStringAsync();
        Assert.False(string.IsNullOrEmpty(shipments));
    }

    [Fact]
    public async Task GetShipmentById_ShouldReturnSuccess()
    {
        var response = await _client.GetAsync("shipment/1");
        response.EnsureSuccessStatusCode();

        var shipment = await response.Content.ReadAsStringAsync();
        Assert.False(string.IsNullOrEmpty(shipment));
    }

    [Fact]
    public async Task GetItemsFromShipment_ShouldReturnSuccess()
    {
        var response = await _client.GetAsync("shipment/1/items");
        response.EnsureSuccessStatusCode();

        var items = await response.Content.ReadAsStringAsync();
        Assert.False(string.IsNullOrEmpty(items));
    }

    [Fact]
    public async Task AddNewShipment_ShouldReturnSuccess()
    {
        var newShipment = new
        {
            OrderId = 123,
            SourceId = 456,
            OrderDate = "2023-10-01T00:00:00Z",
            RequestDate = "2023-10-05T00:00:00Z",
            ShipmentType = "Standard",
            ShipmentStatus = "Pending",
            Notes = "Handle with care",
            CarrierCode = "UPS",
            CarrierDescription = "United Parcel Service",
            ServiceCode = "Ground"
        };

        var response = await _client.PostAsJsonAsync("shipment", newShipment);
        response.EnsureSuccessStatusCode();

        var createdShipment = await response.Content.ReadAsStringAsync();
        Assert.False(string.IsNullOrEmpty(createdShipment));
    }
}