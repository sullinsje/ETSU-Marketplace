using ETSU_Marketplace.Models;
using Microsoft.EntityFrameworkCore;

namespace ETSU_Marketplace.Services;

public class DbItemListingRepository : IItemListingRepository
{
    private readonly ApplicationDbContext _db;
    private readonly IFileStorageService _fss;

    public DbItemListingRepository(ApplicationDbContext db, IFileStorageService fss)
    {
        _db = db;
        _fss = fss;
    }

    public async Task<ICollection<ItemListing>> ReadAllAsync()
    {
        return await _db.ItemListings
        .Include(l => l.Images)
        .ToListAsync();
    }

    public async Task<ItemListing> CreateAsync(ItemListing newItemListing, List<IFormFile> images)
    {
        if (images != null && images.Any())
        {
            foreach (var file in images)
            {
                string path = await _fss.ProcessImageUpload(file);
                newItemListing.Images.Add(new Image { Path = path });
            }
        }

        await _db.ItemListings.AddAsync(newItemListing);
        await _db.SaveChangesAsync();
        return newItemListing;
    }

    public async Task<ItemListing?> ReadAsync(int id)
    {
        return await _db.ItemListings
            .Include(l => l.Images)
            .FirstOrDefaultAsync(l => l.Id == id);
    }

    public async Task UpdateAsync(int oldId, ItemListing updatedItemListing, List<IFormFile> newImages)
    {
        var itemToUpdate = await ReadAsync(oldId);

        if (itemToUpdate == null) return;

        itemToUpdate.Title = updatedItemListing.Title;
        itemToUpdate.Description = updatedItemListing.Description;
        itemToUpdate.Price = updatedItemListing.Price;
        itemToUpdate.Category = updatedItemListing.Category;
        itemToUpdate.Condition = updatedItemListing.Condition;

        if (newImages != null && newImages.Any())
        {
            foreach (var img in itemToUpdate.Images)
            {
                _fss.DeleteImage(img.Path);
            }

            _db.Images.RemoveRange(itemToUpdate.Images);
            itemToUpdate.Images.Clear();

            foreach (var file in newImages)
            {
                var dbPath = await _fss.ProcessImageUpload(file);
                itemToUpdate.Images.Add(new Image { Path = dbPath });
            }
        }

        await _db.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var itemListingToDelete = await ReadAsync(id);
        if (itemListingToDelete != null)
        {
            if (itemListingToDelete.Images != null)
            {
                foreach (var img in itemListingToDelete.Images)
                {
                    var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", img.Path.TrimStart('/'));

                    if (File.Exists(filePath))
                    {
                        File.Delete(filePath);
                    }
                }
            }
            _db.ItemListings.Remove(itemListingToDelete);
            await _db.SaveChangesAsync();
        }
    }

}
