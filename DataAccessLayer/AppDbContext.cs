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

        // PetAd configuration - VARBINARY(MAX) for image (BLOB)
        modelBuilder.Entity<PetAd>(entity =>
        {
            entity.HasKey(p => p.Id);
            entity.Property(p => p.Name).IsRequired().HasMaxLength(100);
            entity.Property(p => p.Species).IsRequired().HasMaxLength(50);
            entity.Property(p => p.City).IsRequired().HasMaxLength(100);
            entity.Property(p => p.Description).HasMaxLength(2000);
            entity.Property(p => p.ImageBytes).HasColumnType("VARBINARY(MAX)");
        });

        // Varsayılan admin (üretimde şifre hash'lenmeli!)
        modelBuilder.Entity<User>().HasData(
            new User { Id = 1, Username = "admin", Password = "admin123" }
        );

        // Örnek ilanlar (fotoğrafsız — kullanıcı sonradan ekleyebilir)
        modelBuilder.Entity<PetAd>().HasData(
            new PetAd { Id = 1, Name = "Pamuk", Species = "Kedi", City = "İstanbul",
                Description = "2 yaşında, çok uysal beyaz tekir bir kedi. Apartman çevresinde kayboldu. Görenler lütfen iletişime geçsin." },
            new PetAd { Id = 2, Name = "Karabaş", Species = "Köpek", City = "Ankara",
                Description = "4 yaşında, eğitimli bir Anadolu çoban köpeği. Bahçeli bir yuvaya sahiplendirilecek." },
            new PetAd { Id = 3, Name = "Minnoş", Species = "Kedi", City = "İzmir",
                Description = "British Shorthair, 1 yaşında. Sokağa alışkın değil, içeride yaşayacağı sevgi dolu bir aile arıyor." },
            new PetAd { Id = 4, Name = "Rex", Species = "Köpek", City = "Bursa",
                Description = "Golden Retriever yavru, 4 aylık. Aşıları tam, oyuncu ve çocuk seviyor." }
        );
    }
}
