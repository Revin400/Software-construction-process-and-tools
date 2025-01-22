using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading.Tasks;
using Moq;
using Moq.Protected;
using Xunit;
using System.Threading;

public class ClientController_Unit_Tests
{
    private readonly HttpClient _client;
    private readonly Mock<HttpMessageHandler> _handlerMock;
    private int _lastCreatedClientId = 999999999;

    public ClientController_Unit_Tests()
    {
        _handlerMock = new Mock<HttpMessageHandler>();
        _client = new HttpClient(_handlerMock.Object) { BaseAddress = new Uri("http://localhost:5000/api/v2/") };
        var apiKey = "a1b2c3d4e5";  
        _client.DefaultRequestHeaders.Add("X-API-KEY", apiKey);
    }

    [Fact]
    public async Task AddNewClient_ShouldReturnSuccess()
    {
        // Arrange
        var newClient = new
        {
            id = _lastCreatedClientId,
            Name = "New Client",
            Address = "123 New Street",
            City = "Any town",
            ZipCode = 54321,
            Province = "Stateville",
            Country = "United States",
            ContactName = "Test1 Client",
            ContactPhone = "0612345678  ",
            ContactEmail = "Test@client.nl"
        };

        var responseMessage = new HttpResponseMessage(System.Net.HttpStatusCode.Created)
        {
            Content = new StringContent("{\"id\":999999999,\"Name\":\"New Client\",\"Address\":\"123 New Street\",\"City\":\"Any town\",\"ZipCode\":54321,\"Province\":\"Stateville\",\"Country\":\"United States\",\"ContactName\":\"Test1 Client\",\"ContactPhone\":\"0612345678\",\"ContactEmail\":\"Test@client.nl\"}")
        };

        _handlerMock.Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>()
            )
            .ReturnsAsync(responseMessage);

        // Act
        var response = await _client.PostAsJsonAsync("client", newClient);
        var createdClient = await response.Content.ReadAsStringAsync();
            
        var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
        var client = JsonSerializer.Deserialize<Client>(createdClient, options);

        // Assert
        response.EnsureSuccessStatusCode();
        Assert.Equal(System.Net.HttpStatusCode.Created, response.StatusCode);
        Assert.False(string.IsNullOrEmpty(createdClient));

        Assert.NotNull(client);
        Assert.NotEqual(0, client.Id);

        _lastCreatedClientId = client.Id;
    }

    [Fact]
    public async Task GetAllClients_ShouldReturnSuccess()
    {
        // Arrange
        var responseMessage = new HttpResponseMessage(System.Net.HttpStatusCode.OK)
        {
            Content = new StringContent("[{\"id\":1, \"Name\":\"Client1\"}, {\"id\":2, \"Name\":\"Client2\"}]")
        };

        _handlerMock.Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>()
            )
            .ReturnsAsync(responseMessage);

        // Act
        var response = await _client.GetAsync("client");

        // Assert
        response.EnsureSuccessStatusCode();
        var clients = await response.Content.ReadAsStringAsync();
        Assert.False(string.IsNullOrEmpty(clients));
    }

    [Fact]
    public async Task GetClientById_ShouldReturnSuccess()
    {
        // Ensure a client has been created
        Assert.NotEqual(0, _lastCreatedClientId);

        // Arrange
        var responseMessage = new HttpResponseMessage(System.Net.HttpStatusCode.OK)
        {
            Content = new StringContent("{\"id\":999999999, \"Name\":\"New Client\"}")
        };

        _handlerMock.Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>()
            )
            .ReturnsAsync(responseMessage);

        // Act
        var response = await _client.GetAsync($"client/{_lastCreatedClientId}");

        // Assert
        response.EnsureSuccessStatusCode();
        var client = await response.Content.ReadAsStringAsync();
        Assert.False(string.IsNullOrEmpty(client));
    }

    [Fact]
    public async Task UpdateClient_ShouldReturnSuccess()
    {
        // Ensure a client has been created
        Assert.NotEqual(0, _lastCreatedClientId);

        // Arrange
        var updatedClient = new
        {
            Name = "Updated Client",
            ContactEmail = "updatedclient@example.com",
            ContactPhone = "0987654321",
            Address = "123 Updated Street",
            City = "Updated town",
            ZipCode = 12345,
            Province = "Updated Stateville",
            Country = "United States",
            ContactName = "Updated Test1 Client",
        };

        var responseMessage = new HttpResponseMessage(System.Net.HttpStatusCode.OK)
        {
            Content = new StringContent("{\"id\":999999999, \"Name\":\"Updated Client\"}")
        };

        _handlerMock.Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>()
            )
            .ReturnsAsync(responseMessage);

        // Act
        var response = await _client.PutAsJsonAsync($"client/{_lastCreatedClientId}", updatedClient);

        // Assert
        response.EnsureSuccessStatusCode();
        var updatedClientResponse = await response.Content.ReadAsStringAsync();
        Assert.False(string.IsNullOrEmpty(updatedClientResponse));
    }

    [Fact]
    public async Task DeleteClient_ShouldReturnSuccess()
    {
        // Ensure a client has been created
        Assert.NotEqual(0, _lastCreatedClientId);

        // Arrange
        var responseMessage = new HttpResponseMessage(System.Net.HttpStatusCode.NoContent);

        _handlerMock.Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>()
            )
            .ReturnsAsync(responseMessage);

        // Act
        var response = await _client.DeleteAsync($"client/{_lastCreatedClientId}");

        // Assert
        response.EnsureSuccessStatusCode();
    }
}
