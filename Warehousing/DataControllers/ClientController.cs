using Microsoft.AspNetCore.Mvc;


[Route("api/client")]
[ApiController]
public class ClientController : ControllerBase
{
    private readonly ClientService _clientService;

    public ClientController(ClientService clientService)
    {
        _clientService = clientService;
    }

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
    public IActionResult GetClientById(int id)
    {
        try
        {
            var clients = _clientService.ReadClientsFromJson();
            var client = clients.FirstOrDefault(c => c.Id == id);
            if (client == null)
            {
                return NotFound($"Client with id {id} not found");
            }
            return Ok(client);
        }
        catch (Exception ex)
        {
            return BadRequest($"Error reading client: {ex.Message}");
        }
    }

    [HttpPost]
    public IActionResult CreateClient([FromBody] Client client)
    {
        try
        {
            var clients = _clientService.ReadClientsFromJson();


            client.Id = _clientService.NextId();
            client.CreatedAt = DateTime.Now;
            client.UpdatedAt = DateTime.Now;

            if(clients.Any(c => c.Id == client.Id))
            {
                return BadRequest($"Client with id {client.Id} already exists");
            }

            if(clients.Any(c => c.ContactEmail == client.ContactEmail))
            {
                return BadRequest($"Client with Email {client.ContactEmail} already exists");
            }

            if(clients.Any(c => c.ContactPhone == client.ContactPhone))
            {
                return BadRequest($"Client with Phone {client.ContactPhone} already exists");
            }

            clients.Add(client);

            _clientService.WriteClientsToJson(clients);

            return Ok(client);

        }
        catch (Exception ex)
        {
            return BadRequest($"Error creating warehouse: {ex.Message}");
        }
    }

    [HttpPut("{id}")]
    public IActionResult UpdateClient(int id, [FromBody] Client client)
    {
        try
        {
            var clients = _clientService.ReadClientsFromJson();
            var existingClient = clients.FirstOrDefault(c => c.Id == id);
            if (existingClient == null)
            {
                return NotFound($"Client with id {id} not found");
            }

            if(clients.Any(c => c.ContactEmail == client.ContactEmail && c.Id != id))
            {
                return BadRequest($"Client with Email {client.ContactEmail} already exists");
            }

            if(clients.Any(c => c.ContactPhone == client.ContactPhone && c.Id != id))
            {
                return BadRequest($"Client with Phone {client.ContactPhone} already exists");
            }

            existingClient.Name = client.Name;
            existingClient.Address = client.Address;
            existingClient.City = client.City;
            existingClient.ZipCode = client.ZipCode;
            existingClient.Province = client.Province;
            existingClient.Country = client.Country;
            existingClient.ContactName = client.ContactName;
            existingClient.ContactEmail = client.ContactEmail;
            existingClient.ContactPhone = client.ContactPhone;
            existingClient.UpdatedAt = DateTime.Now;

            _clientService.WriteClientsToJson(clients);

            return Ok(existingClient);
        }
        catch (Exception ex)
        {
            return BadRequest($"Error updating client: {ex.Message}");
        }
    }

    [HttpDelete("{id}")]
    public IActionResult DeleteClient(int id)
    {
        try
        {
            var clients = _clientService.ReadClientsFromJson();
            var client = clients.FirstOrDefault(c => c.Id == id);
            if (client == null)
            {
                return NotFound($"Client with id {id} not found");
            }
            clients.Remove(client);
            _clientService.WriteClientsToJson(clients);
            return Ok(client);
        }
        catch (Exception ex)
        {
            return BadRequest($"Error deleting client: {ex.Message}");
        }
    }


}