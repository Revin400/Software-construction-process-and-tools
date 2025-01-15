using Microsoft.EntityFrameworkCore;


public class WarehousingContext : DbContext
{
    public WarehousingContext(DbContextOptions<WarehousingContext> options) : base(options) { }

    public WarehousingContext() { }

    public DbSet<Item> Items { get; set; }
    public DbSet<ItemGroup> ItemGroups { get; set; }
    public DbSet<ItemLine> ItemLines { get; set; }
    public DbSet<ItemType> ItemTypes { get; set; }
    public DbSet<Client> Clients { get; set; }
    public DbSet<Location> Locations { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            optionsBuilder.UseSqlite("Data Source=DataSources/Warehousing.db");
        }
    }
}

