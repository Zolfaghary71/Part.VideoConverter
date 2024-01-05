using Moq;
using Part.VideoUploader.Domain;
using Part.VideoUploader.Service.Contracts;
using Xunit;
using Part.VideoUploader.Service.Features.VideoConversion.Commands;
using Part.VideoUploader.Service.Contracts.Infrastructure;


namespace Part.VideoUploader.Test.UnitTests
{
    public class ConvertVideoCommandHandlerTests
    {
        private readonly Mock<IVideoConversionService> _mockVideoConversionService;
        private readonly Mock<IFileUploadInfoRepository> _mockFileUploadInfoRepository;
        private readonly ConvertVideoCommandHandler _handler;

        public ConvertVideoCommandHandlerTests()
        {
            _mockVideoConversionService = new Mock<IVideoConversionService>();
            _mockFileUploadInfoRepository = new Mock<IFileUploadInfoRepository>();
            _handler = new ConvertVideoCommandHandler(_mockVideoConversionService.Object, _mockFileUploadInfoRepository.Object);
        }

        [Fact]
        public async Task Handle_SuccessfulConversion_ReturnsSuccessResponse()
        {

            var command = new ConvertVideoCommand("testUser", "testFile.mp4", new VideoEncodingParameters { /* parameters */ });

            _mockVideoConversionService.Setup(s => s.ConvertAsync(It.IsAny<string>(), It.IsAny<VideoEncodingParameters>()))
                .ReturnsAsync(true);
            
            var response = await _handler.Handle(command, CancellationToken.None);

            Assert.True(response.Success);
            Assert.Equal("Video conversion successful", response.Message);
        }

        [Fact]
        public async Task Handle_FailedConversion_ReturnsFailureResponse()
        {
            
            var command = new ConvertVideoCommand("testUser", "testFile.mp4", new VideoEncodingParameters { /* parameters */ });

            _mockVideoConversionService.Setup(s => s.ConvertAsync(It.IsAny<string>(), It.IsAny<VideoEncodingParameters>()))
                                      .ThrowsAsync(new Exception("Conversion error"));

            var response = await _handler.Handle(command, CancellationToken.None);

            Assert.False(response.Success);
            Assert.Contains("Video conversion failed", response.Message);
        }
    }
}
