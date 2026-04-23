using ETSU_Marketplace.Models;

/// <summary>
/// Extends the generic listing repository with a lease-specific update
/// operation.
/// </summary>

namespace ETSU_Marketplace.Services;

/// <summary>
/// Defines the methods the LeaseListingRepository will use  
/// </summary>
public interface ILeaseListingRepository : IListingRepository<LeaseListing>
{
    // This interface "inherits" other methods
    new Task UpdateAsync(int id, LeaseListing updatedILease, List<IFormFile> images);
}