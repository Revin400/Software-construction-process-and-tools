using Microsoft.EntityFrameworkCore;


public class WarehousingContext : DbContext
{
    // Constructor for dependency injection
    public WarehousingContext(DbContextOptions<WarehousingContext> options) : base(options) { }

    // Parameterless constructor for fallback configuration
    public WarehousingContext() { }

    public DbSet<Item> Items { get; set; }
    public DbSet<ItemGroup> ItemGroups { get; set; }
    public DbSet<ItemLine> ItemLines { get; set; }
    public DbSet<ItemType> ItemTypes { get; set; }

    // Example of other tables that could be added
    // public DbSet<Location> Locations { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        // Use a default SQLite database if no options are provided
        if (!optionsBuilder.IsConfigured)
        {
            optionsBuilder.UseSqlite("Data Source=DataSources/Warehousing.db");
        }
    }
}

