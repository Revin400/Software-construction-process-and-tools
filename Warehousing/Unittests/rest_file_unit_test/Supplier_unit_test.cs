using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading.Tasks;
using Moq;
using Moq.Protected;
using Xunit;
using System.Threading;
using Warehousing.DataControllers_v2;
using Warehousing.DataServices_v2;

public class SupplierController_Unit_Tests
{
    private readonly HttpClient _client;
    private readonly Mock<HttpMessageHandler> _handlerMock;

    public SupplierController_Unit_Tests()
    {
        _handlerMock = new Mock<HttpMessageHandler>();
        _client = new HttpClient(_handlerMock.Object) { BaseAddress = new Uri("http://localhost:5000/api/v2/") };
        var apiKey = "a1b2c3d4e5";  
        _client.DefaultRequestHeaders.Add("X-API-KEY", apiKey);
    }

    [Fact]
    public async Task GetAllSuppliers_ShouldReturnSuccess()
    {
        // Arrange
        var responseMessage = new HttpResponseMessage(System.Net.HttpStatusCode.OK)
        {
            Content = new StringContent("[{\"id\":1, \"code\":\"SUP001\", \"reference\":\"US-SUP001\", \"name\":\"Supplier 1\"}, {\"id\":2, \"code\":\"SUP002\", \"reference\":\"US-SUP002\", \"name\":\"Supplier 2\"}]")
        };

        _handlerMock.Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>()
            )
            .ReturnsAsync(responseMessage);

        // Act
        var response = await _client.GetAsync("supplier");

        // Assert
        response.EnsureSuccessStatusCode();
        var suppliers = await response.Content.ReadAsStringAsync();
        Assert.False(string.IsNullOrEmpty(suppliers));
    }

    [Fact]
    public async Task GetSupplierById_ShouldReturnSuccess()
    {
        // Arrange
        var responseMessage = new HttpResponseMessage(System.Net.HttpStatusCode.OK)
        {
            Content = new StringContent("{\"id\":1, \"code\":\"SUP001\", \"reference\":\"US-SUP001\", \"name\":\"Supplier 1\"}")
        };

        _handlerMock.Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>()
            )
            .ReturnsAsync(responseMessage);

        // Act
        var response = await _client.GetAsync("supplier/1");

        // Assert
        response.EnsureSuccessStatusCode();
        var supplier = await response.Content.ReadAsStringAsync();
        Assert.False(string.IsNullOrEmpty(supplier));
    }

    [Fact]
    public async Task CreateSupplier_ShouldReturnSuccess()
    {
        // Arrange
        var newSupplier = new
        {
            Code = "SUP003",
            Reference = "US-SUP003",
            Name = "Supplier 3"
        };

        var responseMessage = new HttpResponseMessage(System.Net.HttpStatusCode.Created)
        {
            Content = new StringContent("{\"id\":3, \"code\":\"SUP003\", \"reference\":\"US-SUP003\", \"name\":\"Supplier 3\"}")
        };

        _handlerMock.Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>()
            )
            .ReturnsAsync(responseMessage);

        // Act
        var response = await _client.PostAsJsonAsync("supplier", newSupplier);

        // Assert
        response.EnsureSuccessStatusCode();
        var createdSupplier = await response.Content.ReadAsStringAsync();
        Assert.False(string.IsNullOrEmpty(createdSupplier));
    }

    [Fact]
    public async Task UpdateSupplier_ShouldReturnSuccess()
    {
        // Arrange
        var updatedSupplier = new
        {
            Id = 1,
            Code = "SUP001",
            Reference = "US-SUP001",
            Name = "Updated Supplier 1"
        };

        var responseMessage = new HttpResponseMessage(System.Net.HttpStatusCode.OK)
        {
            Content = new StringContent("{\"id\":1, \"code\":\"SUP001\", \"reference\":\"US-SUP001\", \"name\":\"Updated Supplier 1\"}")
        };

        _handlerMock.Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>()
            )
            .ReturnsAsync(responseMessage);

        // Act
        var response = await _client.PutAsJsonAsync("supplier/1", updatedSupplier);

        // Assert
        response.EnsureSuccessStatusCode();
        var updatedSupplierResponse = await response.Content.ReadAsStringAsync();
        Assert.False(string.IsNullOrEmpty(updatedSupplierResponse));
    }

    [Fact]
    public async Task DeleteSupplier_ShouldReturnSuccess()
    {
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
        var response = await _client.DeleteAsync("supplier/1");

        // Assert
        response.EnsureSuccessStatusCode();
    }
}
