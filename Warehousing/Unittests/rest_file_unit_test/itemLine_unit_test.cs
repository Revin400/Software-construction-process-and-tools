using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Xunit;

public class ItemLines_Unit_Tests
{
    private readonly HttpClient _client;

    public ItemLines_Unit_Tests()
    {
        _client = new HttpClient { BaseAddress = new Uri("http://localhost:5000/api/v1/") };
        var apiKey = "a1b2c3d4e5";  
        _client.DefaultRequestHeaders.Add("X-API-KEY", apiKey);
    }

    [Fact]
    public async Task GetAllItemLines_ShouldReturnSuccess()
    {
        var response = await _client.GetAsync("ItemLine");
        response.EnsureSuccessStatusCode();

        var itemLines = await response.Content.ReadAsStringAsync();
        Assert.False(string.IsNullOrEmpty(itemLines));
    }

    [Fact]
    public async Task GetItemLineById_ShouldReturnSuccess()
    {
        var response = await _client.GetAsync("ItemLine/1");
        response.EnsureSuccessStatusCode();

        var itemLine = await response.Content.ReadAsStringAsync();
        Assert.False(string.IsNullOrEmpty(itemLine));
    }

    [Fact]
    public async Task AddNewItemLine_ShouldReturnSuccess()
    {
        var newItemLine = new
        {
            Name = "Construction Materials",
            Description = "A comprehensive range of building and construction materials including cement, bricks, and tiles."
        };

        var response = await _client.PostAsJsonAsync("ItemLine", newItemLine);
        response.EnsureSuccessStatusCode();

        var createdItemLine = await response.Content.ReadAsStringAsync();
        Assert.True(string.IsNullOrEmpty(createdItemLine));
    }

    [Fact]
    public async Task UpdateItemLine_ShouldReturnSuccess()
    {
        var updatedItemLine = new
        {
            Id = 1,
            Name = "Updated Item Line",
            Description = "Updated description of the item line."
        };

        var response = await _client.PutAsJsonAsync("ItemLine/1", updatedItemLine);
        response.EnsureSuccessStatusCode();
    }


    [Fact]
    public async Task DeleteItemLine_ShouldReturnSuccess()
    {
        var response = await _client.DeleteAsync("ItemLine/1");
        response.EnsureSuccessStatusCode();
    }
}
