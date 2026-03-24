using ETSU_Marketplace.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<ChatMessage> ChatMessages => Set<ChatMessage>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // --- Listing Configuration ---
        modelBuilder.Entity<Listing>(entity =>
        {
            entity.HasMany(l => l.Images)
                .WithOne(i => i.Listing)
                .HasForeignKey(i => i.ListingId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasDiscriminator<string>("ListingType")
                .HasValue<ItemListing>("ITEM")
                .HasValue<LeaseListing>("LEASE");
        });

        // --- ApplicationUser Configuration ---
        modelBuilder.Entity<ApplicationUser>()
            .HasOne(u => u.Avatar)
            .WithOne(i => i.User)
            .HasForeignKey<ApplicationUser>(u => u.AvatarId)
            .OnDelete(DeleteBehavior.SetNull);

        // --- FavoriteListing Configuration ---
        modelBuilder.Entity<FavoriteListing>(entity =>
        {
            entity.HasOne(f => f.User)
                .WithMany(u => u.FavoriteListings)
                .HasForeignKey(f => f.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(f => f.Listing)
                .WithMany()
                .HasForeignKey(f => f.ListingId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasIndex(f => new { f.UserId, f.ListingId })
                .IsUnique();
        });
    }

    public DbSet<ItemListing> ItemListings => Set<ItemListing>();
    public DbSet<LeaseListing> LeaseListings => Set<LeaseListing>();
    public DbSet<Listing> Listings => Set<Listing>();
    public DbSet<Image> Images => Set<Image>();
    public DbSet<FavoriteListing> FavoriteListings => Set<FavoriteListing>();
}