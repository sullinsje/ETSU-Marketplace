namespace ETSU_Marketplace.Services;
/// <summary>
/// Defines what methods the FSS will have
/// </summary>
public interface IFileStorageService
{
    public Task<string> ProcessImageUpload(IFormFile file);
    public void DeleteImage(string relativePath);
}