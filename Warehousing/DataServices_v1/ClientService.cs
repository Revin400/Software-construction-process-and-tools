using System;
using System.Collections.Generic;
using System.Linq;

public class ClientService
{
    private readonly WarehousingContext _context;

    public ClientService(WarehousingContext context)
    {
        _context = context;
        _context.Database.EnsureCreated();
    }
    
    public List<Client> GetAllClients()
    {
        return _context.Clients.ToList();
    }

    public Client GetClientById(int id)
    {
        return _context.Clients.FirstOrDefault(c => c.Id == id);
    }

    public void CreateClient(Client client)
    {
        client.CreatedAt = DateTime.Now;
        client.UpdatedAt = DateTime.Now;
        _context.Clients.Add(client);
        _context.SaveChanges();
    }

    public void UpdateClient(Client client)
    {
        _context.ChangeTracker.Clear();
        client.UpdatedAt = DateTime.Now;
        _context.Clients.Update(client);
        _context.SaveChanges();
    }

    public void DeleteClient(int id)
    {
        var client = _context.Clients.FirstOrDefault(c => c.Id == id);
        if (client != null)
        {
            _context.Clients.Remove(client);
            _context.SaveChanges();
        }
    }

    
}