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

        base.OnModelCreating(modelBuilder);

        // --- Listing Configuration ---
        modelBuilder.Entity<Listing>(entity =>
        {
            // Relationship: Listing -> Images (Defined once here)
            entity.HasMany(l => l.Images)
                .WithOne(i => i.Listing)
                .HasForeignKey(i => i.ListingId)
                .OnDelete(DeleteBehavior.Cascade);

            // Inheritance: TPH Discriminator
            entity.HasDiscriminator<string>("ListingType")
                .HasValue<ItemListing>("ITEM")
                .HasValue<LeaseListing>("LEASE");
        });

        // --- ApplicationUser Configuration ---
        modelBuilder.Entity<ApplicationUser>()
            .HasOne(u => u.Avatar)
            .WithOne(i => i.User) // Explicitly link to the User property in Image.cs
            .HasForeignKey<ApplicationUser>(u => u.AvatarId)
            .OnDelete(DeleteBehavior.SetNull);

    }

    // Tables for Item and Lease Listings (for specific queries in Services)
    public DbSet<ItemListing> ItemListings => Set<ItemListing>();
    public DbSet<LeaseListing> LeaseListings => Set<LeaseListing>();

    // Main Listings table (has a DISCRIMINATOR in DB that says which type of listing it is)
    public DbSet<Listing> Listings => Set<Listing>();
    public DbSet<Image> Images => Set<Image>();

}
