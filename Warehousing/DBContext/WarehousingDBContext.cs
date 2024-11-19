using Microsoft.EntityFrameworkCore;


public class WarehousingContext : DbContext
{
    public WarehousingContext(DbContextOptions<WarehousingContext> options) : base(options) { }

    public WarehousingContext() { }

    public DbSet<Item> Items { get; set; }
    public DbSet<ItemGroup> ItemGroups { get; set; }
    public DbSet<ItemLine> ItemLines { get; set; }
    public DbSet<ItemType> ItemTypes { get; set; }

    // Example of other tables that could be added
    // public DbSet<Location> Locations { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        // Use default SQLite database if no options
        if (!optionsBuilder.IsConfigured)
        {
            optionsBuilder.UseSqlite("Data Source=DataSources/Warehousing.db");
        }
    }
}

