using ETSU_Marketplace.Models;

/// <summary>
/// Extends the generic listing repository with a lease-specific update
/// operation.
/// </summary>

namespace ETSU_Marketplace.Services;

public interface ILeaseListingRepository : IListingRepository<LeaseListing>
{
    // This interface "inherits" other methods
    new Task UpdateAsync(int id, LeaseListing updatedILease, List<IFormFile> images);
}