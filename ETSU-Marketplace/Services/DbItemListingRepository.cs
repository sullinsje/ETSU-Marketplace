using ETSU_Marketplace.Models;
using Microsoft.EntityFrameworkCore;

namespace ETSU_Marketplace.Services;

public class DbItemListingRepository : IItemListingRepository
{
    private readonly ApplicationDbContext _db;

    public DbItemListingRepository(ApplicationDbContext db)
    {
        _db = db;
    }

    public async Task<ICollection<ItemListing>> ReadAllAsync()
    {
        return await _db.ItemListings.ToListAsync();
    }

    public async Task<ItemListing> CreateAsync(ItemListing newItemListing)
    {
        await _db.ItemListings.AddAsync(newItemListing);
        await _db.SaveChangesAsync();
        return newItemListing;
    }

}
