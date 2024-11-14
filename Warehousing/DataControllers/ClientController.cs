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
            var clients = _clientService.GetAllClients();
            return Ok(clients);
        }
        catch (Exception ex)
        {
            return BadRequest($"Error reading clients: {ex.Message}");
        }
    }

    [HttpGet("{id}")]
    public IActionResult GetClientById(int id)
    {
        try
        {
            var client = _clientService.GetClientById(id);
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
            var clients = _clientService.GetAllClients();
            if(clients.Any(c => c.ContactEmail == client.ContactEmail))
            {
                return BadRequest($"Client with Email {client.ContactEmail} already exists");
            }

            if(clients.Any(c => c.ContactPhone == client.ContactPhone))
            {
                return BadRequest($"Client with Phone {client.ContactPhone} already exists");
            }

            _clientService.AddClient(client);
            return CreatedAtAction(nameof(GetClientById), new { id = client.Id }, client);
        }
        catch (Exception ex)
        {
            return BadRequest($"Error creating client: {ex.Message}");
        }
    }

    [HttpPut("{id}")]
    public IActionResult UpdateClient(int id, [FromBody] Client client)
    {
        try
        {
            var clients = _clientService.GetAllClients();
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

            client.Id = id;
            _clientService.UpdateClient(client);
            return Ok(client);
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
            var clients = _clientService.GetAllClients();
            var existingClient = clients.FirstOrDefault(c => c.Id == id);
            if (existingClient == null)
            {
                return NotFound($"Client with id {id} not found");
            }

            _clientService.DeleteClient(id);
            return Ok();
        }
        catch (Exception ex)
        {
            return BadRequest($"Error deleting client: {ex.Message}");
        }
    }


}