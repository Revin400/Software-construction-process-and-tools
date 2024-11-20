using System.Collections.Generic;
using System.IO;
using System.Text.Json;

public class LocationService
{
    private readonly WarehousingContext _context;

    public LocationService(WarehousingContext context)
    {
        _context = context;
        _context.Database.EnsureCreated();
    }

    public List<Location> GetAllLocations()
    {
        return _context.Locations.ToList();
    }

    public Location GetLocationById(int id)
    {
        return _context.Locations.FirstOrDefault(l => l.Id == id);
    }

    public void CreateLocation(Location location)
    {
        location.CreatedAt = System.DateTime.Now;
        location.UpdatedAt = System.DateTime.Now;
        _context.Locations.Add(location);
        _context.SaveChanges();
    }

    public void UpdateLocation(Location location)
    {
        _context.ChangeTracker.Clear();
        location.UpdatedAt = System.DateTime.Now;
        _context.Locations.Update(location);
        _context.SaveChanges();
    }

    public void DeleteLocation(int id)
    {
        var location = _context.Locations.FirstOrDefault(l => l.Id == id);
        if (location != null)
        {
            _context.Locations.Remove(location);
            _context.SaveChanges();
        }
    }
}