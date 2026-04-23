namespace ETSU_Marketplace.Services;

/// <summary>
/// Defines a generic set of CRUD operations, user-specific queries, and
/// status management for listing entities.
/// </summary>

public interface IListingRepository<T> where T : Listing
{
    Task<ICollection<T>> ReadAllAsync();
    Task<T> CreateAsync(T newListing, List<IFormFile> image, string userId);
    Task<T?> ReadAsync(int id);
    Task UpdateAsync(int oldId, T updatedListing, List<IFormFile> images);
    Task DeleteAsync(int id);
    Task<ICollection<T>> ReadUserPostsAsync(string userId);
    Task ToggleSoldStatusAsync(int id);
}