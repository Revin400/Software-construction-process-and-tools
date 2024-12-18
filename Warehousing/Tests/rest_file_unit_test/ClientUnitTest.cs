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
    private readonly ClientController _clientController;

    public ClientControllerTests()
    {
        _clientController = new ClientController();
    }

    [Fact]
    public void CreateClient()
    {
        var client = new Client
        {
            Id = 1,
            Name = "Test Client",
            ContactEmail = "adda@df.com",
            ContactPhone = "1234567890"
        };

        var clientJson = JsonSerializer.Serialize(client);
        var jsonDocument = JsonDocument.Parse(clientJson);
        var jsonElement = jsonDocument.RootElement;

        var result = _clientController.Createclient(jsonElement);

        Assert.Equal(201, (result as StatusCodeResult).StatusCode);
    }

    [Fact]
    public void GetAllClients_ReturnsOkResult_WithListOfClients()
    {

        var result = _clientController.GetAllClients();

        Assert.IsType<OkObjectResult>(result);
    }
}