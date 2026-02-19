using ETSU_Marketplace.Models;

namespace ETSU_Marketplace.Services;

public interface ILeaseListingRepository
{
    Task<ICollection<LeaseListing>> ReadAllAsync();
    Task<LeaseListing> CreateAsync(LeaseListing newLeaseListing);
    Task<LeaseListing?> ReadAsync(int id);
    Task UpdateAsync(int oldId, LeaseListing updatedLeaseListing);
    Task DeleteAsync(int id);
}
