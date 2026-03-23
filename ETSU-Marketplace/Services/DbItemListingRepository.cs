using ETSU_Marketplace.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;

namespace ETSU_Marketplace.Services;

/// <summary>
/// This class now inherits from the base ListingRepository, only overriding the Update method
/// because of its unique changes in this class
/// </summary>
public class DbItemListingRepository : DbListingRepository<ItemListing>, IItemListingRepository
{
    public DbItemListingRepository(ApplicationDbContext db, IFileStorageService fss) 
        : base(db, fss) 
    {
       
    }

   public override async Task UpdateAsync(int id, ItemListing updatedItem, List<IFormFile> images)
    {
        // Run all the Title/Price/Image logic from the base class
        await base.UpdateAsync(id, updatedItem, images);

        // Handle the specific fields for Items
        var existing = await _db.ItemListings.FindAsync(id);
        if (existing != null)
        {
            existing.Category = updatedItem.Category;
            existing.Condition = updatedItem.Condition;
            
            await _db.SaveChangesAsync();
        }
    }

}
