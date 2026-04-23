using ETSU_Marketplace.Models;

/// <summary>
/// Extends the generic listing repository with an item-specific update
/// operation.
/// </summary>

namespace ETSU_Marketplace.Services;

public interface IItemListingRepository : IListingRepository<ItemListing>
{
    // This interface "inherits" other methods
    new Task UpdateAsync(int id, ItemListing updatedItem, List<IFormFile> images);
}