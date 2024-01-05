using MediatR;
using Part.VideoUploader.Service.Responses;

namespace Part.VideoUploader.Service.Features.Storage.Commands;

public class UploadFileCommand : IRequest<BaseResponse>
{
    public Stream FileStream { get; }
    public string FileName { get; }
    public string ContentType { get; }
    public string UserId { get; }

    public UploadFileCommand(Stream fileStream, string fileName, string contentType, string userId)
    {
        FileStream = fileStream;
        FileName = fileName;
        ContentType = contentType;
        UserId = userId;
    }
}
