using Microsoft.Extensions.Configuration;
using Part.VideoUploader.Service.Contracts.Infrastructure;
using Minio;

using Minio.DataModel.Args;
using Minio.Exceptions;


namespace Part.VideoUploader.Infrastructure.Storage
{
    public class StorageService : IStorageService
    {
        private readonly IMinioClient _minioClient;
        private readonly string _bucketName;


        public StorageService(IConfiguration configuration)
        {
            var endpoint = configuration["Minio:Endpoint"];
            var accessKey = configuration["Minio:AccessKey"];
            var secretKey = configuration["Minio:SecretKey"];
            _bucketName = configuration["Minio:BucketName"];

            _minioClient = new MinioClient()
                .WithEndpoint(endpoint)
                .WithCredentials(accessKey, secretKey)
                .Build();
        }

        public async Task UploadAsync(string bucketName, string objectName, Stream dataStream, string contentType)
        {
           
            bool found = await _minioClient.BucketExistsAsync(new BucketExistsArgs().WithBucket(_bucketName));
            if (!found)
            {
                await _minioClient.MakeBucketAsync(new MakeBucketArgs().WithBucket(_bucketName));
            }

            
            await _minioClient.PutObjectAsync(
                new PutObjectArgs()
                    .WithBucket(_bucketName)
                    .WithObject(objectName)
                    .WithStreamData(dataStream)
                    .WithObjectSize(dataStream.Length)
                    .WithContentType(contentType)
            );
        }

        public async Task<Stream> DownloadAsync(string objectName)
        {
            try
            {
                var memoryStream = new MemoryStream();
                await _minioClient.GetObjectAsync(
                    new GetObjectArgs()
                        .WithBucket(_bucketName)
                        .WithObject(objectName)
                        .WithCallbackStream(s => s.CopyTo(memoryStream))
                );
                memoryStream.Seek(0, SeekOrigin.Begin); 
                return memoryStream;
            }
            catch (MinioException ex)
            {
                throw new Exception($"Error downloading object '{objectName}': {ex.Message}", ex);
            }
        }
    }
}
