using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Xunit;

public class ItemGroups_Unit_Tests
{
    private readonly HttpClient _client;

    public ItemGroups_Unit_Tests()
    {
        _client = new HttpClient { BaseAddress = new Uri("http://localhost:5000/api/v1/") };
        var apiKey = "a1b2c3d4e5";  
        _client.DefaultRequestHeaders.Add("X-API-KEY", apiKey);
    }

    [Fact]
    public async Task GetAllItemGroups_ShouldReturnSuccess()
    {
        // Arrange & Act
        var response = await _client.GetAsync("itemgroups");
        
        // Assert
        response.EnsureSuccessStatusCode();

        var itemGroups = await response.Content.ReadAsStringAsync();
        Assert.False(string.IsNullOrEmpty(itemGroups));
    }

    [Fact]
    public async Task GetItemGroupById_ShouldReturnSuccess()
    {
        // Arrange & Act
        var response = await _client.GetAsync("itemgroups/1");
        
        // Assert
        response.EnsureSuccessStatusCode();

        var itemGroup = await response.Content.ReadAsStringAsync();
        Assert.False(string.IsNullOrEmpty(itemGroup));
    }

    [Fact]
    public async Task AddNewItemGroup_ShouldReturnSuccess()
    {
        // Arrange
        var newItemGroup = new
        {
            Name = "Test Group",
            Description = "Description for the test group."
        };

        // Act
        var response = await _client.PostAsJsonAsync("itemgroups", newItemGroup);

        // Assert
        response.EnsureSuccessStatusCode();

        var createdItemGroup = await response.Content.ReadAsStringAsync();
        Assert.True(string.IsNullOrEmpty(createdItemGroup));
    }

    [Fact]
    public async Task UpdateItemGroup_ShouldReturnSuccess()
    {
        // Arrange
        var updatedItemGroup = new
        {
            Id = 1,
            Name = "Updated Group Name",
            Description = "Updated group description."
        };

        // Act
        var response = await _client.PutAsJsonAsync("itemgroups/1", updatedItemGroup);

        // Assert
        response.EnsureSuccessStatusCode();
    }

    [Fact]
    public async Task DeleteItemGroup_ShouldReturnSuccess()
    {
        // Act
        var response = await _client.DeleteAsync("itemgroups/1");

        // Assert
        response.EnsureSuccessStatusCode();
    }
}
