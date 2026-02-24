using ETSU_Marketplace.Models;
using Microsoft.EntityFrameworkCore;

namespace ETSU_Marketplace.Services;

public class DbLeaseListingRepository : ILeaseListingRepository
{
    private readonly ApplicationDbContext _db;

    public DbLeaseListingRepository(ApplicationDbContext db)
    {
        _db = db;
    }

    public async Task<ICollection<LeaseListing>> ReadAllAsync()
    {
        return await _db.LeaseListings.ToListAsync();
    }

    public async Task<LeaseListing> CreateAsync(LeaseListing newLeaseListing)
    {
        await _db.LeaseListings.AddAsync(newLeaseListing);
        await _db.SaveChangesAsync();
        return newLeaseListing;
    }

    public async Task<LeaseListing?> ReadAsync(int id)
    {
        return await _db.LeaseListings.FindAsync(id);
    }

    public async Task UpdateAsync(int oldId, LeaseListing updatedLeaseListing)
    {
        var LeaseListingToUpdate = await ReadAsync(oldId);
        if (LeaseListingToUpdate != null)
        {
            LeaseListingToUpdate.Address = updatedLeaseListing.Address;
            LeaseListingToUpdate.LeaseStart = updatedLeaseListing.LeaseStart;
            LeaseListingToUpdate.LeaseEnd = updatedLeaseListing.LeaseEnd;
            LeaseListingToUpdate.UtilitiesIncluded = updatedLeaseListing.UtilitiesIncluded;
            LeaseListingToUpdate.Title = updatedLeaseListing.Title;
            LeaseListingToUpdate.Description = updatedLeaseListing.Description;
            LeaseListingToUpdate.Price = updatedLeaseListing.Price;
            await _db.SaveChangesAsync();
        }
    }
    
    public async Task DeleteAsync(int id)
    {
        var leaseListingToDelete = await ReadAsync(id);
        if (leaseListingToDelete != null)
        {
            _db.LeaseListings.Remove(leaseListingToDelete);
            await _db.SaveChangesAsync();
        }
    }

}
