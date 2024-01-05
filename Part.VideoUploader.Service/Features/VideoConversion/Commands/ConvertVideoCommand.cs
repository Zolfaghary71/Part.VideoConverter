using MediatR;
using Part.VideoUploader.Domain;
using Part.VideoUploader.Service.Responses;

namespace Part.VideoUploader.Service.Features.VideoConversion.Commands;

public class ConvertVideoCommand:IRequest<BaseResponse>
{
    public string UserId { get; set; }
    public string FileName { get; }
    public VideoEncodingParameters EncodingParameters { get; }

    public ConvertVideoCommand(string userId, string fileName, VideoEncodingParameters encodingParameters)
    {
        UserId = userId;
        FileName = fileName;
        EncodingParameters = encodingParameters;
    }
}