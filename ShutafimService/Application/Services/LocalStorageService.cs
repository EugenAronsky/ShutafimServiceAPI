using ShutafimService.Application.Interfaces;

namespace ShutafimService.Application.Services
{
    public class LocalStorageService : IStorageService
    {
        private readonly string _basePath;
        private readonly string _publicUrlPrefix;

        public LocalStorageService(IWebHostEnvironment env, IConfiguration config)
        {
            _basePath = Path.Combine(env.WebRootPath ?? "wwwroot", "uploads");
            _publicUrlPrefix = "/uploads"; // maps to wwwroot/uploads/
        }

        public async Task<string> UploadAsync(IFormFile file, string path)
        {
            var fullPath = Path.Combine(_basePath, path);
            var directory = Path.GetDirectoryName(fullPath)!;

            Directory.CreateDirectory(directory);

            using var stream = new FileStream(fullPath, FileMode.Create);
            await file.CopyToAsync(stream);

            return $"{_publicUrlPrefix}/{path.Replace("\\", "/")}";
        }

        public Task DeleteAsync(string fileUrl)
        {
            var relativePath = fileUrl.Replace(_publicUrlPrefix, "").TrimStart('/');
            var fullPath = Path.Combine(_basePath, relativePath.Replace("/", Path.DirectorySeparatorChar.ToString()));

            if (File.Exists(fullPath))
                File.Delete(fullPath);

            return Task.CompletedTask;
        }

        public Task<Stream> GetFileAsync(string fileUrl)
        {
            var relativePath = fileUrl.Replace(_publicUrlPrefix, "").TrimStart('/');
            var fullPath = Path.Combine(_basePath, relativePath.Replace("/", Path.DirectorySeparatorChar.ToString()));

            if (!File.Exists(fullPath))
                throw new FileNotFoundException();

            Stream stream = new FileStream(fullPath, FileMode.Open, FileAccess.Read);
            return Task.FromResult(stream);
        }

        public Task<List<string>> ListFilesAsync(string prefix)
        {
            var folderPath = Path.Combine(_basePath, prefix);
            var result = new List<string>();

            if (Directory.Exists(folderPath))
            {
                foreach (var file in Directory.GetFiles(folderPath))
                {
                    var relative = Path.GetRelativePath(_basePath, file);
                    result.Add($"{_publicUrlPrefix}/{relative.Replace("\\", "/")}");
                }
            }

            return Task.FromResult(result);
        }
    }
}
