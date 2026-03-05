using ETSU_Marketplace.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

public class ApplicationDbContext : IdentityDbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<ChatMessage> ChatMessages => Set<ChatMessage>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Listing>()
            .HasMany(l => l.Images)
            .WithOne(i => i.Listing)
            .HasForeignKey(i => i.ListingId)
            .OnDelete(DeleteBehavior.Cascade); // Deleting a listing deletes its images

        modelBuilder.Entity<Listing>()
            .HasDiscriminator<string>("ListingType") // Changes column name to "ListingType"
            .HasValue<ItemListing>("ITEM")           // Stores "ITEM" instead of class name
            .HasValue<LeaseListing>("LEASE");        // Stores "LEASE"
    }

    // Tables for Item and Lease Listings (for specific queries in Services)
    public DbSet<ItemListing> ItemListings => Set<ItemListing>();
    public DbSet<LeaseListing> LeaseListings => Set<LeaseListing>();

    // Main Listings table (has a DISCRIMINATOR in DB that says which type of listing it is)
    public DbSet<Listing> Listings => Set<Listing>();
    public DbSet<Image> Images => Set<Image>();

}
