namespace ETSU_Marketplace.Services;

/// <summary>
/// Provides functionality for handling file storage operations.
/// </summary>

public interface IFileStorageService
{
    public Task<string> ProcessImageUpload(IFormFile file);
    public void DeleteImage(string relativePath);
}