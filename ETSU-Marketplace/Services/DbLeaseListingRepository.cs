namespace ETSU_Marketplace.Services;

/// <summary>
/// Contains the necessary methods for CRUD operations regarding LeaseListings
/// NOTE:
/// This class now inherits from the base ListingRepository, only overriding the Update method
/// because of its unique changes in this class
/// See DbListingRepository.cs for general implementation
/// </summary>
public class DbLeaseListingRepository : DbListingRepository<LeaseListing>, ILeaseListingRepository
{
    public DbLeaseListingRepository(ApplicationDbContext db, IFileStorageService fss)
        : base(db, fss)
    {

    }

    public override async Task UpdateAsync(int id, LeaseListing updatedLease, List<IFormFile> images)
    {
        // Run all the Title/Price/Image logic from the base class
        await base.UpdateAsync(id, updatedLease, images);

        // Handle the specific fields for Leases
        var existing = await _db.LeaseListings.FindAsync(id);
        if (existing != null)
        {
            existing.Address = updatedLease.Address;
            existing.LeaseStart = updatedLease.LeaseStart;
            existing.LeaseEnd = updatedLease.LeaseEnd;
            existing.UtilitiesIncluded = updatedLease.UtilitiesIncluded;

            await _db.SaveChangesAsync();
        }
    }

}
