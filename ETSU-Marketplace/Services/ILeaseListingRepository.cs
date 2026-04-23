using ETSU_Marketplace.Models;

namespace ETSU_Marketplace.Services;

/// <summary>
/// Defines the methods the LeaseListingRepository will use  
/// </summary>
public interface ILeaseListingRepository : IListingRepository<LeaseListing>
{
    // This interface "inherits" other methods
    new Task UpdateAsync(int id, LeaseListing updatedILease, List<IFormFile> images);
}
