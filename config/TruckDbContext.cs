using Microsoft.EntityFrameworkCore;
using fleetmanagement.entities;

namespace fleetmanagement.config;

public class TruckDbContext : DbContext
{
    public TruckDbContext(DbContextOptions<TruckDbContext> options) : base(options) { }

    public DbSet<Truck> Trucks { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Truck>().HasKey(t => t.Id);
        base.OnModelCreating(modelBuilder);
    }
}
