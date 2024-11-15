using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Xunit;

public class Location_Unit_Tests
{
    private readonly HttpClient _client;

    public Location_Unit_Tests()
    {
        _client = new HttpClient { BaseAddress = new Uri("http://localhost:5000/api/") };
    }

    [Fact]
    public async Task GetAllLocations_ShouldReturnSuccess()
    {
        var response = await _client.GetAsync("location");
        response.EnsureSuccessStatusCode();

        var locations = await response.Content.ReadAsStringAsync();
        Assert.False(string.IsNullOrEmpty(locations));
    }

    [Fact]
    public async Task CreateNewLocation_ShouldReturnSuccess()
    {
        var newLocation = new
        {
            code = "A.1.0",
            name = "Row: A, Rack: 1, Shelf: 0",
            warehouse_id = 1
        };

        var response = await _client.PostAsJsonAsync("location", newLocation);
        response.EnsureSuccessStatusCode();

        var createdLocation = await response.Content.ReadAsStringAsync();
        Assert.False(string.IsNullOrEmpty(createdLocation));
    }

    [Fact]
    public async Task UpdateLocation_ShouldReturnSuccess()
    {
        var updatedLocation = new
        {
            code = "A.1.1",
            name = "Row: A, Rack: 1, Shelf: 1",
            warehouse_id = 1
        };

        var response = await _client.PutAsJsonAsync("location/1", updatedLocation);
        response.EnsureSuccessStatusCode();

        var updatedLocationResponse = await response.Content.ReadAsStringAsync();
        Assert.False(string.IsNullOrEmpty(updatedLocationResponse));
    }

    [Fact]
    public async Task DeleteLocation_ShouldReturnSuccess()
    {
        var response = await _client.DeleteAsync("location/1");
        response.EnsureSuccessStatusCode();

        var deleteResponse = await response.Content.ReadAsStringAsync();
        Assert.False(string.IsNullOrEmpty(deleteResponse));
    }
}