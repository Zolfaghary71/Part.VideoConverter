
using MediatR;
using Part.VideoUploader.Service.Contracts;
using Part.VideoUploader.Service.Contracts.Infrastructure;
using Part.VideoUploader.Service.Responses;

namespace Part.VideoUploader.Service.Features.VideoConversion.Commands;

public class ConvertVideoCommandHandler : IRequestHandler<ConvertVideoCommand, BaseResponse>
{
    private readonly IVideoConversionService _videoConversionService;
    private readonly IFileUploadInfoRepository _fileUploadInfoRepository;

    public ConvertVideoCommandHandler(IVideoConversionService videoConversionService, IFileUploadInfoRepository fileUploadInfoRepository)
    {
        _videoConversionService = videoConversionService;
        _fileUploadInfoRepository = fileUploadInfoRepository;
    }

    public async Task<BaseResponse> Handle(ConvertVideoCommand request, CancellationToken cancellationToken)
    {
        try
        {
            await _videoConversionService.ConvertAsync(request.FileName, request.EncodingParameters);

            return new BaseResponse("Video conversion successful", true);
        }
        catch (Exception ex)
        {
            return new BaseResponse($"Video conversion failed: {ex.Message}", false);
        }
    }
}
