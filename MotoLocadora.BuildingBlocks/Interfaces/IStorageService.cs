namespace MotoLocadora.BuildingBlocks.Interfaces;


public interface IStorageService
{
    Task<string> UploadAsync(Stream fileStream, string fileName, string contentType);
    Task<Stream?> GetAsync(string fileName);
    Task DeleteAsync(string fileName);
}