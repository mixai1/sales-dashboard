using Microsoft.EntityFrameworkCore;
using Portal.Dal.EntityConfigurations;
using Portal.Entities;

namespace Portal.Dal;

public class PortalDbContext : DbContext {
    public PortalDbContext(DbContextOptions options) : base(options) {
        ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
    }

    public PortalDbContext() { }

    public DbSet<Sale> Sales { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder) {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfiguration(new SaleConfiguration());
    }
}
