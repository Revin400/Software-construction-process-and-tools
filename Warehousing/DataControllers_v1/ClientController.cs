using Microsoft.AspNetCore.Mvc;
using Warehousing.DataServices_v1;



namespace Warehousing.DataControllers_v1
{



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
        public ActionResult<List<Client>> Get()
        {
            return _clientService.GetAllClients();
        }

        [HttpGet("{id}")]
        public ActionResult<Client> Get(int id)
        {
            var client = _clientService.GetClientById(id);
            if (client == null)
            {
                return NotFound();
            }
            return client;
        }

        [HttpPost]
        public IActionResult Post([FromBody] Client client)
        {
            try
            {
                var clients = _clientService.GetAllClients();
                if (clients.Any(c => c.ContactEmail == client.ContactEmail))
                {
                    return BadRequest($"Client with Email {client.ContactEmail} already exists");
                }

                if (clients.Any(c => c.ContactPhone == client.ContactPhone))
                {
                    return BadRequest($"Client with Phone {client.ContactPhone} already exists");
                }

                _clientService.CreateClient(client);
                return CreatedAtAction(nameof(Get), new { id = client.Id }, client);
            }
            catch (Exception ex)
            {
                return BadRequest($"Error creating client: {ex.Message}");
            }
        }

        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody] Client client)
        {
            try
            {
                var clients = _clientService.GetAllClients();
                var existingClient = clients.FirstOrDefault(c => c.Id == id);
                if (existingClient == null)
                {
                    return NotFound($"Client with id {id} not found");
                }

                if (clients.Any(c => c.ContactEmail == client.ContactEmail && c.Id != id))
                {
                    return BadRequest($"Client with Email {client.ContactEmail} already exists");
                }

                if (clients.Any(c => c.ContactPhone == client.ContactPhone && c.Id != id))
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
}