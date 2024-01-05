namespace Part.VideoUploader.Service.Contracts.Infrastructure;

public interface IStorageService
{
   Task UploadAsync(string bucketName, string objectName, Stream dataStream, string contentType);

}