using Microsoft.EntityFrameworkCore;
using PetFinder.Models;

namespace PetFinder.DataAccessLayer;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<User> Users => Set<User>();
    public DbSet<PetAd> PetAds => Set<PetAd>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // User configuration
        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(u => u.Id);
            entity.Property(u => u.Username).IsRequired().HasMaxLength(50);
            entity.Property(u => u.Password).IsRequired().HasMaxLength(200);
            entity.HasIndex(u => u.Username).IsUnique();
        });

        // PetAd configuration - VARBINARY(MAX) for images
        modelBuilder.Entity<PetAd>(entity =>
        {
            entity.HasKey(p => p.Id);
            entity.Property(p => p.Name).IsRequired().HasMaxLength(100);
            entity.Property(p => p.Species).IsRequired().HasMaxLength(50);
            entity.Property(p => p.City).IsRequired().HasMaxLength(100);
            entity.Property(p => p.Description).HasMaxLength(2000);
            entity.Property(p => p.ImageBytes).HasColumnType("VARBINARY(MAX)");
        });

        // Seed default admin user (NOTE: in production, hash passwords!)
        modelBuilder.Entity<User>().HasData(
            new User { Id = 1, Username = "admin", Password = "admin123" }
        );
    }
}
