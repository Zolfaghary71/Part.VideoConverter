using Moq;
using Part.VideoUploader.Domain;
using Part.VideoUploader.Service.Contracts.Infrastructure;
using Part.VideoUploader.Service.Features.Storage.Commands;
using Xunit;

namespace Part.VideoUploader.Test.UnitTests;

public class UploadFileCommandHandlerTest
{
    [Fact]
    public async Task Handle_SuccessfulUpload_ReturnsSuccessResponse()
    {
        var mockStorageService = new Mock<IStorageService>();
        var mockFileUploadInfoRepository = new Mock<IFileUploadInfoRepository>();
        var fileStream = new MemoryStream();
        var command = new UploadFileCommand(fileStream, "file.mp4", "video/mp4", Guid.NewGuid().ToString());
        var handler = new UploadFileCommandHandler(mockStorageService.Object, mockFileUploadInfoRepository.Object);

        var response = await handler.Handle(command, new CancellationToken());

        mockFileUploadInfoRepository.Verify(repo => repo.AddAsync(It.IsAny<FileUploadInfo>()), Times.Once);
        mockStorageService.Verify(service => service.UploadAsync("defaultbucket", "file.mp4", fileStream, "video/mp4"), Times.Once);
        mockFileUploadInfoRepository.Verify(repo => repo.UpdateAsync(It.IsAny<FileUploadInfo>()), Times.Once);
        Assert.True(response.Success);
        Assert.Equal("chunk received", response.Message);
    }

    [Fact]
    public async Task Handle_UploadFails_ReturnsErrorResponse()
    {
        var mockStorageService = new Mock<IStorageService>();
        mockStorageService.Setup(service => service.UploadAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<Stream>(), It.IsAny<string>()))
                          .ThrowsAsync(new Exception("Upload failed"));
        var mockFileUploadInfoRepository = new Mock<IFileUploadInfoRepository>();
        var fileStream = new MemoryStream();
        var command = new UploadFileCommand(fileStream, "file.mp4", "video/mp4", Guid.NewGuid().ToString());
        var handler = new UploadFileCommandHandler(mockStorageService.Object, mockFileUploadInfoRepository.Object);

        var response = await handler.Handle(command, new CancellationToken());

        mockFileUploadInfoRepository.Verify(repo => repo.AddAsync(It.IsAny<FileUploadInfo>()), Times.Once);
        mockFileUploadInfoRepository.Verify(repo => repo.UpdateAsync(It.IsAny<FileUploadInfo>()), Times.Once);
        Assert.False(response.Success);
        Assert.Equal("chunk Failed", response.Message);
    }
}