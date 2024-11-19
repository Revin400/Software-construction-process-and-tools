using Microsoft.EntityFrameworkCore;

public class TransferDBContext : DbContext
{
    public DbSet<Transfer> Transfers { get; set; }
    public DbSet<TransferItems> TransferItems { get; set; }

    // Example of other tables that could be added
    // public DbSet<Location> Locations { get; set; }
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlite("Data Source=DataSources/Transfers.db");
    }
}
