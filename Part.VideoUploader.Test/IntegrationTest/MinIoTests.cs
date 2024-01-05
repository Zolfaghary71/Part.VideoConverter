using Microsoft.Extensions.Configuration;
using Part.VideoUploader.Infrastructure.Storage;
using Xunit;

namespace Part.VideoUploader.Test.IntegrationTest;


public class MinIoTests
{
    [Fact]
    public async void SaveToMinIo()
    {
        var configuration = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json") 
            .Build();

        var service = new StorageService(configuration);

        string testFilePath = "path/to/local/testvideo.mp4"; 
        string objectName = "testvideo.mp4";
        using var fileStream = File.OpenRead(testFilePath); 

        await service.UploadAsync("defaultbucket", objectName, fileStream, "video/mp4");
    }
}
