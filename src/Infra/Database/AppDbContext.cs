using Api.Domain.UseCases.Customers.Models;
using Api.Domain.UseCases.Pdvs.Models;


namespace Api.Infra.Database;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<Customer> Customer { get; set; } = null!;
    public DbSet<Pdv> Pdv { get; set; } = null!;
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // PersonSeeder.Seed(modelBuilder);
        // NotificationSeeder.Seed(modelBuilder);
    }

}
