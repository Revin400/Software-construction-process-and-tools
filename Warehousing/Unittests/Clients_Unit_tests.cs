using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Xunit;

public class Clients_Unit_Tests
{
    private readonly HttpClient _client;

    public Clients_Unit_Tests()
    {
        _client = new HttpClient { BaseAddress = new Uri("http://localhost:5000/api/") };
    }

    [Fact]
    public async Task AddNewClient_ShouldReturnSuccess()
    {
        var newClient = new
        {        
            Id = 1,  
            Name = "Acme Corporation",

            Address = "123 Main Street",

             City = "Anytown",

            ZipCode = 1234,

            Province = "Stateville",

            Country = "United States",

            ContactName = "John Doe",

            ContactPhone = $"+1 12345{new Random().Next(1000, 9999)}",

            ContactEmail = $"john@acmecorp{new Random().Next(10, 99)}.com"

          
        };

        var response = await _client.PostAsJsonAsync("client", newClient);
        response.EnsureSuccessStatusCode();

        var createdClient = await response.Content.ReadAsStringAsync();
        Assert.False(string.IsNullOrEmpty(createdClient));
    }

    [Fact]
    public async Task GetAllClients_ShouldReturnSuccess()
    {
        var response = await _client.GetAsync("client");
        response.EnsureSuccessStatusCode();

        var clients = await response.Content.ReadAsStringAsync();
        Assert.False(string.IsNullOrEmpty(clients));
    }

    [Fact]
    public async Task GetClientById_ShouldReturnSuccess()
    {
        var response = await _client.GetAsync("client/1");
        response.EnsureSuccessStatusCode();

        var client = await response.Content.ReadAsStringAsync();
        Assert.False(string.IsNullOrEmpty(client));
    }

    [Fact]
    public async Task UpdateClient_ShouldReturnSuccess()
    {
        var updatedClient = new
        {
            Name = "Updated - Acme Corporation",
            Address = "Updated - 123 Main Street",
            City = "Updated - Anytown",
            ZipCode = 12345,
            Province = "Updated - Stateville",
            Country = "Updated - United States",
            ContactName = "Updated - Jane Doe",
            ContactPhone = $"+1 664666{new Random().Next(1000, 9999)}",
            ContactEmail = $"updated_email{new Random().Next(99, 999)}@dd.com",
        };

        var response = await _client.PutAsJsonAsync("client/1", updatedClient);
        response.EnsureSuccessStatusCode();

        var updatedClientResponse = await response.Content.ReadAsStringAsync();
        Assert.False(string.IsNullOrEmpty(updatedClientResponse));
    }

    [Fact]
    public async Task DeleteClient_ShouldReturnSuccess()
    {
        var response = await _client.DeleteAsync("client/1");
        response.EnsureSuccessStatusCode();
    }
  
}