using ETSU_Marketplace.Models;

namespace ETSU_Marketplace.Services;

public interface IItemListingRepository
{
    Task<ICollection<ItemListing>> ReadAllAsync();
    Task<ItemListing> CreateAsync(ItemListing newItemListing, List<IFormFile> images);
    Task<ItemListing?> ReadAsync(int id);
    Task UpdateAsync(int oldId, ItemListing updatedItemListing, List<IFormFile> images);
    Task DeleteAsync(int id);
}
