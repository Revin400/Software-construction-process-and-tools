using Microsoft.EntityFrameworkCore;

public class WarehousingContext : DbContext
{
    public DbSet<Item> Items { get; set; }
    public DbSet<ItemGroup> ItemGroups { get; set; }
    public DbSet<ItemLine> ItemLines { get; set; }
    public DbSet<ItemType> ItemTypes { get; set; }
    public DbSet<Client> Clients { get; set; }

    public DbSet<Location> Locations { get; set; }

    // Example of other tables that could be added
    // public DbSet<Location> Locations { get; set; }
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlite("Data Source=DataSources/Warehousing.db");
    }
}

