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

    public async Task<ItemListing?> ReadAsync(int id)
    {
        return await _db.ItemListings.FindAsync(id);
    }

    public async Task UpdateAsync(int oldId, ItemListing updatedItemListing)
    {
        var ItemListingToUpdate = await ReadAsync(oldId);
        if (ItemListingToUpdate != null)
        {
            ItemListingToUpdate.Category = updatedItemListing.Category;
            ItemListingToUpdate.Condition = updatedItemListing.Condition;
            ItemListingToUpdate.Title = updatedItemListing.Title;
            ItemListingToUpdate.Description = updatedItemListing.Description;
            ItemListingToUpdate.Price = updatedItemListing.Price;
            await _db.SaveChangesAsync();
        }
    }
    
    public async Task DeleteAsync(int id)
    {
        var itemListingToDelete = await ReadAsync(id);
        if (itemListingToDelete != null)
        {
            _db.ItemListings.Remove(itemListingToDelete);
            await _db.SaveChangesAsync();
        }
    }

}
