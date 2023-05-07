using Amazon.S3;
using Amazon.S3.Model;
using Amazon.S3.Transfer;
using Bookstore.Domain;
using Microsoft.Extensions.Configuration;
using System.IO;
using System.Threading.Tasks;

namespace Bookstore.Data.FileServices
{
    public class S3FileService : IFileService
    {
        private readonly IConfiguration configuration;
        private readonly TransferUtility transferUtility;

        public S3FileService(IConfiguration configuration, IAmazonS3 s3Client)
        {
            this.configuration = configuration;
            transferUtility = new TransferUtility(s3Client);
        }

        public async Task DeleteAsync(string filePath)
        {
            if (string.IsNullOrWhiteSpace(filePath)) return;

            string bucketName = configuration["AWS:BucketName"];
            DeleteObjectRequest request = new DeleteObjectRequest
            {
                BucketName = bucketName,
                Key = Path.GetFileName(filePath)
            };

            await transferUtility.S3Client.DeleteObjectAsync(request);
        }

        public async Task<string> SaveAsync(Stream contents, string filename)
        {
            if (contents == null) return null;

            string bucketName = configuration["AWS:BucketName"];
            string uniqueFilename = $"{Path.GetFileNameWithoutExtension(Path.GetRandomFileName())}{Path.GetExtension(filename)}";
            string cloudFrontDomain = configuration["AWS:CloudFrontDomain"];

            TransferUtilityUploadRequest request = new TransferUtilityUploadRequest
            {
                BucketName = bucketName,
                InputStream = contents,
                Key = uniqueFilename
            };

            await transferUtility.UploadAsync(request);

            return $"{cloudFrontDomain}/{uniqueFilename}";
        }
    }
}
