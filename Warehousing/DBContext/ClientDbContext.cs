using Microsoft.EntityFrameworkCore;

public class ClientDbContext : DbContext
{
    public DbSet<Client> Clients { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlite("Data Source=DataSources/clients.db");
    }
}
