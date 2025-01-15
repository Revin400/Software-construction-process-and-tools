using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using Warehousing.DataControllers_v1;
using Warehousing.DataServices_v1;
using Xunit;

public class ClientControllerTests
{
    private readonly Mock<IClientService> _mockClientService;
    private readonly ClientController _clientController;

    private readonly string _testFilePath = Path.GetTempFileName();

    public ClientControllerTests()
    {
        _mockClientService = new Mock<IClientService>();
        _clientController = new ClientController(_mockClientService.Object);
    }

    [Fact]
    public void GetAllClients_ReturnsOkResult_WithListOfClients()
    {
        // Arrange
        var clients = new List<JsonElement>
        {
            JsonDocument.Parse("{\"Id\":1,\"Name\":\"Test Client\"}").RootElement
        };
        _mockClientService.Setup(service => service.ReadClientsFromJson()).Returns(clients);

        // Act
        var result = _clientController.GetAllClients();

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var returnedClients = Assert.IsType<List<JsonElement>>(okResult.Value);
        Assert.Single(returnedClients);
    }

    [Fact]
    public void CreateClient_ReturnsCreatedResult()
    {
        // Arrange
        var clientJson = "{\"Id\":1,\"Name\":\"Test Client\"}";
        var jsonElement = JsonDocument.Parse(clientJson).RootElement;
        var clients = new List<JsonElement>();

        _mockClientService.Setup(service => service.ReadClientsFromJson()).Returns(clients);
        _mockClientService.Setup(service => service.WriteClientsToJson(It.IsAny<List<JsonElement>>()));

        // Act
        var result = _clientController.Createclient(jsonElement);

        // Assert
        var statusCodeResult = Assert.IsType<StatusCodeResult>(result);
        Assert.Equal(201, statusCodeResult.StatusCode);
    }
}
