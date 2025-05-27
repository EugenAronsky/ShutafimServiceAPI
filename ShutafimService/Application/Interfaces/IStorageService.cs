namespace ShutafimService.Application.Interfaces
{
    public interface IStorageService
    {
        Task<string> UploadAsync(IFormFile file, string path);
        Task DeleteAsync(string fileUrl);
        Task<Stream> GetFileAsync(string fileUrl);
        Task<List<string>> ListFilesAsync(string prefix);
    }
}
