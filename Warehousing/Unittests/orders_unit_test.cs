using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Xunit;

public class Orders_Unit_Tests
{
    private readonly HttpClient _client;

    public Orders_Unit_Tests()
    {
        _client = new HttpClient { BaseAddress = new Uri("http://localhost:5000/api/") };
    }

    [Fact]
    public async Task GetAllOrders_ShouldReturnSuccess()
    {
        var response = await _client.GetAsync("orders");
        response.EnsureSuccessStatusCode();

        var orders = await response.Content.ReadAsStringAsync();
        Assert.False(string.IsNullOrEmpty(orders));
    }

    [Fact]
    public async Task GetOrderById_ShouldReturnSuccess()
    {
        var response = await _client.GetAsync("orders/1");
        response.EnsureSuccessStatusCode();

        var order = await response.Content.ReadAsStringAsync();
        Assert.False(string.IsNullOrEmpty(order));
    }

    [Fact]
    public async Task CreateNewOrder_ShouldReturnSuccess()
    {
        var newOrder = new
        {
            Id = 1,
            sourceId = 101,
            orderDate = "2024-05-01T13:45:00Z",
            requestDate = "2024-05-05T00:00:00Z",
            reference = "ORD001",
            referenceExtra = "First bulk order",
            orderStatus = "Packed",
            notes = "Please ensure timely delivery.",
            shippingNotes = "Handle with care.",
            pickingNotes = "Verify items before dispatch.",
            warehouseId = 3,
            shipTo = 1,
            billTo = 2,
            shipmentId = 1,
            totalAmount = 5000.00,
            totalDiscount = 100.00,
            totalTax = 475.00,
            totalSurcharge = 30.00,
            items = new[]
            {
                new { itemId = 1, amount = 50 },
                new { itemId = 2, amount = 30 }
            }
        };

        var response = await _client.PostAsJsonAsync("orders", newOrder);
        response.EnsureSuccessStatusCode();

        var createdOrder = await response.Content.ReadAsStringAsync();
        Assert.False(string.IsNullOrEmpty(createdOrder));
    }

    [Fact]
    public async Task UpdateOrder_ShouldReturnSuccess()
    {
        var updatedOrder = new
        {
            id = 1,
            sourceId = 101,
            orderDate = "2023-05-01T13:45:00Z",
            requestDate = "2024-05-05T00:00:00Z",
            reference = "ORD001",
            referenceExtra = "First bulk order",
            orderStatus = "Shipped",
            notes = "Please ensure timely delivery.",
            shippingNotes = "Handle with care.",
            pickingNotes = "Verify items before dispatch.",
            warehouseId = 3,
            shipTo = 1,
            billTo = 2,
            shipmentId = 1,
            totalAmount = 100.00,
            totalDiscount = 100.00,
            totalTax = 45.00,
            totalSurcharge = 30.00,
            items = new[]
            {
                new { itemId = 1, amount = 50 },
                new { itemId = 2, amount = 30 }
            }
        };

        var response = await _client.PutAsJsonAsync("orders/1", updatedOrder);
        response.EnsureSuccessStatusCode();

        var updatedOrderResponse = await response.Content.ReadAsStringAsync();
        Assert.False(string.IsNullOrEmpty(updatedOrderResponse));
    }

    [Fact]
    public async Task DeleteOrder_ShouldReturnSuccess()
    {
        var response = await _client.DeleteAsync("orders/2");
        response.EnsureSuccessStatusCode();

        var deleteResponse = await response.Content.ReadAsStringAsync();
        Assert.False(string.IsNullOrEmpty(deleteResponse));
    }
}