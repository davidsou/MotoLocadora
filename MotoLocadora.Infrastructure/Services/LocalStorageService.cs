using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using MotoLocadora.BuildingBlocks.Interfaces;

namespace MotoLocadora.Infrastructure.Services;



public class LocalStorageService : IStorageService
{
    private readonly string _basePath;
    private readonly string _publicBaseUrl;

    public LocalStorageService(IWebHostEnvironment env, IConfiguration config)
    {
        _basePath = Path.Combine(env.ContentRootPath, "Storage");
        _publicBaseUrl = config["LocalStorage:BaseUrl"] ?? "http://localhost:5000/storage";

        if (!Directory.Exists(_basePath))
            Directory.CreateDirectory(_basePath);
    }

    public async Task<string> UploadAsync(Stream fileStream, string fileName, string contentType)
    {
        var filePath = Path.Combine(_basePath, fileName);

        using var output = File.Create(filePath);
        await fileStream.CopyToAsync(output);

        return $"{_publicBaseUrl}/{fileName}";
    }

    public async Task<Stream?> GetAsync(string fileName)
    {
        var filePath = Path.Combine(_basePath, fileName);

        if (!File.Exists(filePath))
            return null;

        var memory = new MemoryStream();
        using var stream = new FileStream(filePath, FileMode.Open, FileAccess.Read);
        await stream.CopyToAsync(memory);
        memory.Position = 0;
        return memory;
    }

    public Task DeleteAsync(string fileName)
    {
        var filePath = Path.Combine(_basePath, fileName);

        if (File.Exists(filePath))
            File.Delete(filePath);

        return Task.CompletedTask;
    }
}
