using Microsoft.Extensions.Logging;
using Moq;
using Part.VideoUploader.Infrastructure.Storage;
using Part.VideoUploader.Service.Contracts.Infrastructure;
using Part.VideoUploader.Service.Features.Storage;
using Part.VideoUploader.Service.Features.Storage.Commands;
using Xunit;

namespace Part.VideoUploader.Test.UnitTests;

public class StorageServiceTests
{
    [Fact]
    public async Task SaveToStorage()
    {
        var storageServiceMock = new Mock<IStorageService>();
        var fileUploadRepository = new Mock<IFileUploadInfoRepository>();
        var fileStream = new MemoryStream(); 
        var fileName = "file.mp4";
        var contentType = "video/mp4";
        var userId = Guid.NewGuid().ToString(); 
        var bucketName = "defaultbucket";

        var command = new UploadFileCommand(fileStream, fileName, contentType, userId);
        var handler = new UploadFileCommandHandler(storageServiceMock.Object, fileUploadRepository.Object);

        await handler.Handle(command, new CancellationToken());

        storageServiceMock.Verify(s => s.UploadAsync(bucketName, fileName, fileStream, contentType), Times.Once);
    }
}

