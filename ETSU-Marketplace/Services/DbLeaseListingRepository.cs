using ETSU_Marketplace.Models;
using Microsoft.EntityFrameworkCore;

namespace ETSU_Marketplace.Services;

public class DbLeaseListingRepository : ILeaseListingRepository
{
    private readonly ApplicationDbContext _db;
    private readonly IFileStorageService _fss;

    public DbLeaseListingRepository(ApplicationDbContext db, IFileStorageService fss)
    {
        _db = db;
        _fss = fss;
    }

    public async Task<ICollection<LeaseListing>> ReadAllAsync()
    {
        return await _db.LeaseListings
        .Include(l => l.Images)
        .ToListAsync();
    }

    public async Task<LeaseListing> CreateAsync(LeaseListing newLeaseListing, List<IFormFile> images, string userId)
    {
        newLeaseListing.UserId = userId;

        if (images != null && images.Any())
        {
            foreach (var file in images)
            {
                string path = await _fss.ProcessImageUpload(file);

                newLeaseListing.Images.Add(new Image { Path = path });
            }
        }

        await _db.LeaseListings.AddAsync(newLeaseListing);
        await _db.SaveChangesAsync();
        return newLeaseListing;
    }

    public async Task<LeaseListing?> ReadAsync(int id)
    {
        return await _db.LeaseListings
            .Include(l => l.User)
            .Include(l => l.Images)
            .FirstOrDefaultAsync(l => l.Id == id);
    }

    public async Task UpdateAsync(int oldId, LeaseListing updatedLeaseListing, List<IFormFile> newImages)
    {
        var leaseListingToUpdate = await ReadAsync(oldId);

        if (leaseListingToUpdate == null) return;

        leaseListingToUpdate.Address = updatedLeaseListing.Address;
        leaseListingToUpdate.LeaseStart = updatedLeaseListing.LeaseStart;
        leaseListingToUpdate.LeaseEnd = updatedLeaseListing.LeaseEnd;
        leaseListingToUpdate.UtilitiesIncluded = updatedLeaseListing.UtilitiesIncluded;
        leaseListingToUpdate.Title = updatedLeaseListing.Title;
        leaseListingToUpdate.Description = updatedLeaseListing.Description;
        leaseListingToUpdate.Price = updatedLeaseListing.Price;

        if (newImages != null && newImages.Any())
        {
            foreach (var img in leaseListingToUpdate.Images)
            {
                _fss.DeleteImage(img.Path);
            }

            _db.Images.RemoveRange(leaseListingToUpdate.Images);
            leaseListingToUpdate.Images.Clear();

            foreach (var file in newImages)
            {
                var dbPath = await _fss.ProcessImageUpload(file);
                leaseListingToUpdate.Images.Add(new Image { Path = dbPath });
            }
        }

        await _db.SaveChangesAsync();
    }


    public async Task DeleteAsync(int id)
    {
        var leaseListingToDelete = await ReadAsync(id);
        if (leaseListingToDelete != null)
        {
            if (leaseListingToDelete.Images != null)
            {
                foreach (var img in leaseListingToDelete.Images)
                {
                    var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", img.Path.TrimStart('/'));

                    if (File.Exists(filePath))
                    {
                        File.Delete(filePath);
                    }
                }
            }
            _db.LeaseListings.Remove(leaseListingToDelete);
            await _db.SaveChangesAsync();
        }
    }

    public async Task<ICollection<LeaseListing>> ReadUserPostsAsync(string userId)
    {
        return await _db.LeaseListings
            .Include(l => l.Images)
            .Where(l => l.UserId == userId)
            .OrderByDescending(l => l.CreatedAt)
            .ToListAsync();
    }

}
