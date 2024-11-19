using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Xunit;

public class ItemTypes_Unit_Tests
{
    private readonly HttpClient _client;

    public ItemTypes_Unit_Tests()
    {
        _client = new HttpClient { BaseAddress = new Uri("http://localhost:5000/api/") };
    }

    [Fact]
    public async Task GetAllItemTypes_ShouldReturnSuccess()
    {
        var response = await _client.GetAsync("itemtypes");
        response.EnsureSuccessStatusCode();

        var itemTypes = await response.Content.ReadAsStringAsync();
        Assert.False(string.IsNullOrEmpty(itemTypes));
    }

    [Fact]
    public async Task GetItemTypeById_ShouldReturnSuccess()
    {
        var response = await _client.GetAsync("itemtypes/1");
        response.EnsureSuccessStatusCode();

        var itemType = await response.Content.ReadAsStringAsync();
        Assert.False(string.IsNullOrEmpty(itemType));
    }

    [Fact]
    public async Task AddNewItemType_ShouldReturnSuccess()
    {
        var newItemType = new
        {
            Name = "Test Tool",
            Description = "Test tool description"
        };

        var response = await _client.PostAsJsonAsync("itemtypes", newItemType);
        response.EnsureSuccessStatusCode();

        var createdItemType = await response.Content.ReadAsStringAsync();
        Assert.False(string.IsNullOrEmpty(createdItemType));
    }

    [Fact]
    public async Task UpdateItemType_ShouldReturnSuccess()
    {
        var updatedItemType = new
        {
            Id = 1,
            Name = "Updated Tool",
            Description = "Updated description"
        };

        var response = await _client.PutAsJsonAsync("itemtypes/1", updatedItemType);
        response.EnsureSuccessStatusCode();
    }

    [Fact]
    public async Task DeleteItemType_ShouldReturnSuccess()
    {
        var response = await _client.DeleteAsync("itemtypes/1");
        response.EnsureSuccessStatusCode();
    }
}
