using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading.Tasks;
using Moq;
using Moq.Protected;
using Xunit;
using System.Threading;

public class ItemGroups_Unit_Tests
{
    private readonly HttpClient _client;
    private readonly Mock<HttpMessageHandler> _handlerMock;
    private int _lastCreatedItemGroupId = 999999999;

    public ItemGroups_Unit_Tests()
    {
        _handlerMock = new Mock<HttpMessageHandler>();
        _client = new HttpClient(_handlerMock.Object) { BaseAddress = new Uri("http://localhost:5000/api/v1/") };
        var apiKey = "a1b2c3d4e5";  
        _client.DefaultRequestHeaders.Add("X-API-KEY", apiKey);
    }

    [Fact]
    public async Task GetAllItemGroups_ShouldReturnSuccess()
    {
        // Arrange
        var responseMessage = new HttpResponseMessage(System.Net.HttpStatusCode.OK)
        {
            Content = new StringContent("[{\"id\":1, \"name\":\"Group 1\"}, {\"id\":2, \"name\":\"Group 2\"}]")
        };

        _handlerMock.Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>()
            )
            .ReturnsAsync(responseMessage);

        // Act
        var response = await _client.GetAsync("itemgroups");

        // Assert
        response.EnsureSuccessStatusCode();
        var itemGroups = await response.Content.ReadAsStringAsync();
        Assert.False(string.IsNullOrEmpty(itemGroups));
    }

    [Fact]
    public async Task AddNewItemGroup_ShouldReturnSuccess()
    {
        // Arrange
        var newItemGroup = new ItemGroup  
        {
            Id = _lastCreatedItemGroupId,
            Name = "New Item Group test",
            Description = "This is a new item group test.",
            CreatedAt = DateTime.Now,
            UpdatedAt = DateTime.Now
        };

        var responseMessage = new HttpResponseMessage(System.Net.HttpStatusCode.Created)
        {
            Content = new StringContent("{\"id\":999999999, \"name\":\"New Item Group test\", \"description\":\"This is a new item group test.\"}")
        };

        _handlerMock.Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>()
            )
            .ReturnsAsync(responseMessage);

        // Act
        var response = await _client.PostAsJsonAsync("itemgroups", newItemGroup);
        var createdItemGroup = await response.Content.ReadAsStringAsync();

        // Assert
        response.EnsureSuccessStatusCode();
        Assert.Equal(System.Net.HttpStatusCode.Created, response.StatusCode);
    }

    [Fact]
    public async Task GetItemGroupById_ShouldReturnSuccess()
    {
        // Ensure an item group has been created
        Assert.NotEqual(0, _lastCreatedItemGroupId);

        // Arrange
        var responseMessage = new HttpResponseMessage(System.Net.HttpStatusCode.OK)
        {
            Content = new StringContent("{\"id\":999999999, \"name\":\"New Item Group test\", \"description\":\"This is a new item group test.\"}")
        };

        _handlerMock.Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>()
            )
            .ReturnsAsync(responseMessage);

        // Act
        var response = await _client.GetAsync($"itemgroups/{_lastCreatedItemGroupId}");

        // Assert
        response.EnsureSuccessStatusCode();
        var itemGroup = await response.Content.ReadAsStringAsync();
        Assert.False(string.IsNullOrEmpty(itemGroup));
    }

    [Fact]
    public async Task UpdateItemGroup_ShouldReturnSuccess()
    {
        // Ensure an item group has been created
        Assert.NotEqual(0, _lastCreatedItemGroupId);

        // Arrange
        var updatedItemGroup = new
        {
            Id = _lastCreatedItemGroupId,
            Name = "Updated Group Name",
            Description = "Updated group description.",
            created_at = "2024-12-14 06:58:09",
            updated_at = "2024-12-14 06:58:09"
        };

        var responseMessage = new HttpResponseMessage(System.Net.HttpStatusCode.OK)
        {
            Content = new StringContent("{\"id\":999999999, \"name\":\"Updated Group Name\", \"description\":\"Updated group description.\"}")
        };

        _handlerMock.Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>()
            )
            .ReturnsAsync(responseMessage);

        // Act
        var response = await _client.PutAsJsonAsync($"itemgroups/{_lastCreatedItemGroupId}", updatedItemGroup);

        // Assert
        response.EnsureSuccessStatusCode();
    }

    [Fact]
    public async Task DeleteItemGroup_ShouldReturnSuccess()
    {
        // Ensure an item group has been created
        Assert.NotEqual(0, _lastCreatedItemGroupId);

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
        var response = await _client.DeleteAsync($"itemgroups/{_lastCreatedItemGroupId}");

        // Assert
        response.EnsureSuccessStatusCode();
    }
}
