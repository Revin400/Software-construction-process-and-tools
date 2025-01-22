using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Moq;
using Moq.Protected;
using Xunit;
using System.Threading;

public class ItemLines_Unit_Tests
{
    private readonly HttpClient _client;
    private readonly Mock<HttpMessageHandler> _handlerMock;

    public ItemLines_Unit_Tests()
    {
        _handlerMock = new Mock<HttpMessageHandler>();
        _client = new HttpClient(_handlerMock.Object) { BaseAddress = new Uri("http://localhost:5000/api/v1/") };
        var apiKey = "a1b2c3d4e5";  
        _client.DefaultRequestHeaders.Add("X-API-KEY", apiKey);
    }

    [Fact]
    public async Task GetAllItemLines_ShouldReturnSuccess()
    {
        var responseMessage = new HttpResponseMessage(System.Net.HttpStatusCode.OK)
        {
            Content = new StringContent("[{\"id\":1,\"name\":\"Item1\"}]")
        };

        _handlerMock.Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>()
            )
            .ReturnsAsync(responseMessage);

        var response = await _client.GetAsync("ItemLine");
        response.EnsureSuccessStatusCode();

        var itemLines = await response.Content.ReadAsStringAsync();
        Assert.False(string.IsNullOrEmpty(itemLines));
    }

    [Fact]
    public async Task GetItemLineById_ShouldReturnSuccess()
    {
        var responseMessage = new HttpResponseMessage(System.Net.HttpStatusCode.OK)
        {
            Content = new StringContent("{\"id\":1,\"name\":\"Item1\"}")
        };

        _handlerMock.Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>()
            )
            .ReturnsAsync(responseMessage);

        var response = await _client.GetAsync("ItemLine/1");
        response.EnsureSuccessStatusCode();

        var itemLine = await response.Content.ReadAsStringAsync();
        Assert.False(string.IsNullOrEmpty(itemLine));
    }

    [Fact]
    public async Task AddNewItemLine_ShouldReturnSuccess()
    {
        var responseMessage = new HttpResponseMessage(System.Net.HttpStatusCode.OK)
        {
            Content = new StringContent("")
        };

        _handlerMock.Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>()
            )
            .ReturnsAsync(responseMessage);

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
        var responseMessage = new HttpResponseMessage(System.Net.HttpStatusCode.OK);

        _handlerMock.Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>()
            )
            .ReturnsAsync(responseMessage);

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
        var responseMessage = new HttpResponseMessage(System.Net.HttpStatusCode.OK);

        _handlerMock.Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>()
            )
            .ReturnsAsync(responseMessage);

        var response = await _client.DeleteAsync("ItemLine/1");
        response.EnsureSuccessStatusCode();
    }
}