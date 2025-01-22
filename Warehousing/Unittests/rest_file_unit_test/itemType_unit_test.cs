using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading.Tasks;
using Moq;
using Moq.Protected;
using Xunit;
using System.Threading;

public class ItemTypes_Unit_Tests
{
    private readonly HttpClient _client;
    private readonly Mock<HttpMessageHandler> _handlerMock;

    public ItemTypes_Unit_Tests()
    {
        _handlerMock = new Mock<HttpMessageHandler>();
        _client = new HttpClient(_handlerMock.Object) { BaseAddress = new Uri("http://localhost:5000/api/v1/") };
        var apiKey = "a1b2c3d4e5";  
        _client.DefaultRequestHeaders.Add("X-API-KEY", apiKey);
    }

    [Fact]
    public async Task GetAllItemTypes_ShouldReturnSuccess()
    {
        // Arrange
        var responseMessage = new HttpResponseMessage(System.Net.HttpStatusCode.OK)
        {
            Content = new StringContent("[{\"id\":1, \"name\":\"Tool A\", \"description\":\"Description of Tool A\"}, {\"id\":2, \"name\":\"Tool B\", \"description\":\"Description of Tool B\"}]")
        };

        _handlerMock.Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>()
            )
            .ReturnsAsync(responseMessage);

        // Act
        var response = await _client.GetAsync("itemtype");

        // Assert
        response.EnsureSuccessStatusCode();
        var itemTypes = await response.Content.ReadAsStringAsync();
        Assert.False(string.IsNullOrEmpty(itemTypes));
    }

    [Fact]
    public async Task GetItemTypeById_ShouldReturnSuccess()
    {
        // Arrange
        var responseMessage = new HttpResponseMessage(System.Net.HttpStatusCode.OK)
        {
            Content = new StringContent("{\"id\":1, \"name\":\"Tool A\", \"description\":\"Description of Tool A\"}")
        };

        _handlerMock.Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>()
            )
            .ReturnsAsync(responseMessage);

        // Act
        var response = await _client.GetAsync("itemtype/1");

        // Assert
        response.EnsureSuccessStatusCode();
        var itemType = await response.Content.ReadAsStringAsync();
        Assert.False(string.IsNullOrEmpty(itemType));
    }

    [Fact]
    public async Task AddNewItemType_ShouldReturnSuccess()
    {
        // Arrange
        var newItemType = new
        {
            Name = "Test Tool",
            Description = "Test tool description"
        };

        var responseMessage = new HttpResponseMessage(System.Net.HttpStatusCode.Created)
        {
            Content = new StringContent("{\"id\":3, \"name\":\"Test Tool\", \"description\":\"Test tool description\"}")
        };

        _handlerMock.Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>()
            )
            .ReturnsAsync(responseMessage);

        // Act
        var response = await _client.PostAsJsonAsync("itemtype", newItemType);

        // Assert
        response.EnsureSuccessStatusCode();
        var createdItemType = await response.Content.ReadAsStringAsync();
        Assert.False(string.IsNullOrEmpty(createdItemType));
    }

    [Fact]
    public async Task UpdateItemType_ShouldReturnSuccess()
    {
        // Arrange
        var updatedItemType = new
        {
            Id = 1,
            Name = "Updated Tool",
            Description = "Updated description"
        };

        var responseMessage = new HttpResponseMessage(System.Net.HttpStatusCode.OK)
        {
            Content = new StringContent("{\"id\":1, \"name\":\"Updated Tool\", \"description\":\"Updated description\"}")
        };

        _handlerMock.Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>()
            )
            .ReturnsAsync(responseMessage);

        // Act
        var response = await _client.PutAsJsonAsync("itemtype/1", updatedItemType);

        // Assert
        response.EnsureSuccessStatusCode();
        var updatedItemTypeResponse = await response.Content.ReadAsStringAsync();
        Assert.False(string.IsNullOrEmpty(updatedItemTypeResponse));
    }

    [Fact]
    public async Task DeleteItemType_ShouldReturnSuccess()
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
        var response = await _client.DeleteAsync("itemtype/1");

        // Assert
        response.EnsureSuccessStatusCode();
    }
}
