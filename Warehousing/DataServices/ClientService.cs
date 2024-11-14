using System;
using System.Collections.Generic;
using System.Linq;

public class ClientService
{
    private readonly ClientDbContext _context;

    public ClientService(ClientDbContext context)
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
        return _context.Clients.Find(id);
    }

    public void AddClient(Client client)
    {
        client.CreatedAt = DateTime.Now;
        client.UpdatedAt = DateTime.Now;
        _context.Clients.Add(client);
        _context.SaveChanges();
    }

    public void UpdateClient(Client client)
    {
        client.UpdatedAt = DateTime.Now;
        _context.Clients.Update(client);
        _context.SaveChanges();
    }

    public void DeleteClient(int id)
    {
        var client = _context.Clients.Find(id);
        if (client != null)
        {
            _context.Clients.Remove(client);
            _context.SaveChanges();
        }
    }
}
