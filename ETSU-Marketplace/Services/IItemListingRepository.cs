using ETSU_Marketplace.Models;

namespace ETSU_Marketplace.Services;

public interface IItemListingRepository
{
    Task<ICollection<ItemListing>> ReadAllAsync();
    Task<ItemListing> CreateAsync(ItemListing newItemListing);
}
