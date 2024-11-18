using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Xunit;

public class Items_Unit_Tests
{
    private readonly HttpClient _client;

    public Items_Unit_Tests()
    {
        _client = new HttpClient { BaseAddress = new Uri("http://localhost:5000/api/") };
    }

    [Fact]
    public async Task GetAllItems_ShouldReturnSuccess()
    {
        // Arrange & Act
        var response = await _client.GetAsync("items");
        
        // Assert
        response.EnsureSuccessStatusCode();

        var items = await response.Content.ReadAsStringAsync();
        Assert.False(string.IsNullOrEmpty(items));
    }

    [Fact]
    public async Task GetItemById_ShouldReturnSuccess()
    {
        // Arrange & Act
        var response = await _client.GetAsync("items/1");
        
        // Assert
        response.EnsureSuccessStatusCode();

        var item = await response.Content.ReadAsStringAsync();
        Assert.False(string.IsNullOrEmpty(item));
    }

    [Fact]
    public async Task AddNewItem_ShouldReturnSuccess()
    {
        // Arrange
        var newItem = new
        {
            Code = "NEW123",
            Description = "New high-quality item",
            ShortDescription = "New Item",
            UpcCode = "987654321012",
            ModelNumber = "NI-2024",
            CommodityCode = "NEW-COMMODITY",
            ItemLineId = 1,
            ItemGroupId = 1,
            ItemTypeId = 1,
            UnitPurchaseQuantity = 1,
            UnitOrderQuantity = 1,
            PackOrderQuantity = 1,
            SupplierId = 1,
            SupplierCode = "SUPNEW",
            SupplierPartNumber = "NI-2024-PART",
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        // Act
        var response = await _client.PostAsJsonAsync("items", newItem);

        // Assert
        response.EnsureSuccessStatusCode();

        var createdItem = await response.Content.ReadAsStringAsync();
        Assert.False(string.IsNullOrEmpty(createdItem));
    }

    [Fact]
    public async Task UpdateItem_ShouldReturnSuccess()
    {
        // Arrange
        var updatedItem = new
        {
            Id = 1,
            Code = "UPDATED123",
            Description = "Updated description for item",
            ShortDescription = "Updated Item",
            UpcCode = "987654321013",
            ModelNumber = "UI-2024",
            CommodityCode = "UPDATED-COMMODITY",
            ItemLineId = 1,
            ItemGroupId = 2,
            ItemTypeId = 1,
            UnitPurchaseQuantity = 2,
            UnitOrderQuantity = 2,
            PackOrderQuantity = 2,
            SupplierId = 2,
            SupplierCode = "SUPUPDATED",
            SupplierPartNumber = "UI-2024-PART",
            CreatedAt = DateTime.UtcNow.AddDays(-1),
            UpdatedAt = DateTime.UtcNow
        };

        // Act
        var response = await _client.PutAsJsonAsync("items/1", updatedItem);

        // Assert
        response.EnsureSuccessStatusCode();
    }

    [Fact]
    public async Task DeleteItem_ShouldReturnSuccess()
    {
        // Act
        var response = await _client.DeleteAsync("items/1");

        // Assert
        response.EnsureSuccessStatusCode();
    }
}
