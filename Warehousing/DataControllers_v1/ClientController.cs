using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using Warehousing.DataServices_v1;

namespace Warehousing.DataControllers_v1
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class ClientController : ControllerBase
    {
        private readonly ClientService _clientService = new ClientService();
        private readonly string _filePath = Path.Combine(Directory.GetCurrentDirectory(), "Datasources", "Clients.json");

        [HttpGet]
        public IActionResult GetAllClients()
        {
            try
            {
                var clients = _clientService.ReadClientsFromJson();
                return Ok(clients);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error reading clients: {ex.Message}");
            }
        }

        [HttpGet("{id}")]
        public IActionResult GetclientById(int id)
        {
            try
            {
                var clients = _clientService.ReadClientsFromJson();
                if (id < 0 || id >= clients.Count)
                {
                    return StatusCode(200, $"null");
                }
                return Ok(clients[id]);
            }
            catch (Exception ex)
            {
                return BadRequest($"Error reading clients: {ex.Message}");
            }
        }

        [HttpPost]
        public IActionResult Createclient([FromBody] JsonElement client)
        {
            try
            {
                var clients = _clientService.ReadClientsFromJson();

                var clientWithId = JsonSerializer.Serialize(client);
                clients.Add(JsonDocument.Parse(clientWithId).RootElement);

                _clientService.WriteClientsToJson(clients);
                return StatusCode(201);
            }
            catch (Exception ex)
            {
                return BadRequest($"Error creating clients: {ex.Message}");
            }
        }

        [HttpPut("{id}")]
        public IActionResult Updateclient(int id, [FromBody] JsonElement client)
        {
            try
            {
                var clients = _clientService.ReadClientsFromJson();
                var updatedClient = JsonSerializer.Serialize(client);
                clients[id - 1] = JsonDocument.Parse(updatedClient).RootElement;

                _clientService.WriteClientsToJson(clients);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest($"Error updating client: {ex.Message}");
            }
        }

        [HttpDelete("{id}")]
        public IActionResult Deleteclient(int id)
        {          
            System.IO.File.WriteAllText(_filePath, "[]");
            return Ok();
           
            
        }
    }
}