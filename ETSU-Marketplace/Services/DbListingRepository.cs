using Microsoft.EntityFrameworkCore;
using ETSU_Marketplace.Models;

namespace ETSU_Marketplace.Services;

/// <summary>
/// This class is a new repository pattern I am implementing (Generic Repository Pattern).
/// Since Items and Leases require super similar logic and functionality, they will both 
/// inherit from this generic repository. The only difference between Item and Lease is the update
/// method, so you have to do a method override to perform the unique code for each update. 
/// All methods are marked as virtual since this is the base class and the methods may need to be 
/// overridden.
/// </summary>
/// <typeparam name="T">T should be the listing type when you make a repository that inherits from
/// this base class. For example, a ItemListing repository is just a ListingRepo of type ItemListing
/// </typeparam>
public class DbListingRepository<T> : IListingRepository<T> where T : Listing
{
    protected readonly ApplicationDbContext _db;
    private readonly IFileStorageService _fss;

    public DbListingRepository(ApplicationDbContext db, IFileStorageService fss)
    {
        _db = db;
        _fss = fss;
    }

    public virtual async Task<T> CreateAsync(T newListing, List<IFormFile> images, string userId)
    {
        newListing.UserId = userId;

        if (images != null && images.Any())
        {
            foreach (var file in images)
            {
                string path = await _fss.ProcessImageUpload(file);

                newListing.Images.Add(new Image { Path = path });
            }
        }

        await _db.Set<T>().AddAsync(newListing);
        await _db.SaveChangesAsync();
        return newListing;
    }

    public virtual async Task DeleteAsync(int id)
    {
        var listingToDelete = await ReadAsync(id);
        if (listingToDelete != null)
        {
            if (listingToDelete.Images != null)
            {
                foreach (var img in listingToDelete.Images)
                {
                    var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", img.Path.TrimStart('/'));

                    if (File.Exists(filePath))
                    {
                        File.Delete(filePath);
                    }
                }
            }
            _db.Set<T>().Remove(listingToDelete);
            await _db.SaveChangesAsync();
        }
    }

    public virtual async Task<ICollection<T>> ReadAllAsync()
    {
        return await _db.Set<T>()
            .Include(l => l.Images)
            .Include(l => l.User)
            .ToListAsync();
    }

    public virtual async Task<T?> ReadAsync(int id)
    {
        return await _db.Set<T>()
            .Include(l => l.Images)
            .Include(l => l.User)
                .ThenInclude(u => u!.Avatar)
            .FirstOrDefaultAsync(l => l.Id == id);
    }

    public virtual async Task<ICollection<T>> ReadUserPostsAsync(string userId)
    {
        return await _db.Set<T>()
            .Include(l => l.Images)
            .Include(l => l.User)
            .Where(l => l.UserId == userId)
            .OrderByDescending(l => l.CreatedAt)
            .ToListAsync();
    }

    public virtual async Task UpdateAsync(int id, T updatedListing, List<IFormFile> newImages)
    {
        var existing = await _db.Set<T>()
            .Include(l => l.Images)
            .FirstOrDefaultAsync(l => l.Id == id);

        if (existing == null) return;

        existing.Title = updatedListing.Title;
        existing.Description = updatedListing.Description;
        existing.Price = updatedListing.Price;

        if (newImages != null && newImages.Any())
        {
            foreach (var img in existing.Images)
            {
                _fss.DeleteImage(img.Path);
            }

            _db.Images.RemoveRange(existing.Images);
            existing.Images.Clear();

            foreach (var file in newImages)
            {
                var dbPath = await _fss.ProcessImageUpload(file);
                existing.Images.Add(new Image { Path = dbPath });
            }
        }
    }
}
