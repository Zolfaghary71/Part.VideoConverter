using MediatR;
using Part.VideoUploader.Domain;
using Part.VideoUploader.Service.Contracts.Infrastructure;
using Part.VideoUploader.Service.Responses;

namespace Part.VideoUploader.Service.Features.Storage.Commands;

public class UploadFileCommandHandler : IRequestHandler<UploadFileCommand, BaseResponse>
{
    private readonly IStorageService _storageService;
    private readonly IFileUploadInfoRepository _fileUploadInfoRepository;

    public UploadFileCommandHandler(IStorageService storageService, IFileUploadInfoRepository fileUploadInfoRepository)
    {
        _storageService = storageService;
        _fileUploadInfoRepository = fileUploadInfoRepository;
    }

    public async Task<BaseResponse> Handle(UploadFileCommand request, CancellationToken cancellationToken)
    {
        var uploadId = Guid.NewGuid();
        var uploadInfo = new FileUploadInfo
        {
            Id = uploadId,
            FileName = request.FileName,
            UserId = request.UserId,
            Size = request.FileStream.Length,
            UploadStartTime = DateTime.UtcNow,
            Status = "Uploading"
        };

        await _fileUploadInfoRepository.AddAsync(uploadInfo);

        try
        {
            await _storageService.UploadAsync("defaultbucket", request.FileName, request.FileStream, request.ContentType);
            uploadInfo.Status = "Completed";
            uploadInfo.UploadEndTime = DateTime.UtcNow;
        }
        catch (Exception ex)
        {
            uploadInfo.Status = "Failed";
            await _fileUploadInfoRepository.UpdateAsync(uploadInfo);
            return new BaseResponse("chunk Failed",false);
        }

        await _fileUploadInfoRepository.UpdateAsync(uploadInfo);

        return new BaseResponse("chunk received",true);
    }
}