using Part.VideoUploader.Domain;

namespace Part.VideoUploader.Service.Contracts;

public interface IVideoConversionService
{
    Task<bool> ConvertAsync(string fileName, VideoEncodingParameters videoEncodingParameters);
}