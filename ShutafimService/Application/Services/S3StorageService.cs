using ShutafimService.Application.Interfaces;
using Amazon.S3;
using Amazon.S3.Model;

namespace ShutafimService.Application.Services
{
    public class S3StorageService : IStorageService
    {
        private readonly IAmazonS3 _s3Client;
        private readonly string _bucketName;
        private readonly string _region;

        public S3StorageService(IAmazonS3 s3Client, IConfiguration config)
        {
            _s3Client = s3Client;
            _bucketName = config["Storage:S3:BucketName"]!;
            _region = config["Storage:S3:Region"]!;
        }

        public async Task<string> UploadAsync(IFormFile file, string path)
        {
            using var stream = file.OpenReadStream();

            var request = new PutObjectRequest
            {
                BucketName = _bucketName,
                Key = path,
                InputStream = stream,
                ContentType = file.ContentType,
                CannedACL = S3CannedACL.PublicRead
            };

            await _s3Client.PutObjectAsync(request);

            return $"https://{_bucketName}.s3.{_region}.amazonaws.com/{path}";
        }

        public async Task DeleteAsync(string fileUrl)
        {
            var uri = new Uri(fileUrl);
            var key = uri.AbsolutePath.TrimStart('/');

            await _s3Client.DeleteObjectAsync(_bucketName, key);
        }

        public async Task<Stream> GetFileAsync(string fileUrl)
        {
            var uri = new Uri(fileUrl);
            var key = uri.AbsolutePath.TrimStart('/');

            var response = await _s3Client.GetObjectAsync(_bucketName, key);
            return response.ResponseStream;
        }

        public async Task<List<string>> ListFilesAsync(string prefix)
        {
            var result = new List<string>();

            var request = new ListObjectsV2Request
            {
                BucketName = _bucketName,
                Prefix = prefix
            };

            var response = await _s3Client.ListObjectsV2Async(request);
            foreach (var obj in response.S3Objects)
            {
                result.Add($"https://{_bucketName}.s3.{_region}.amazonaws.com/{obj.Key}");
            }

            return result;
        }
    }
}
