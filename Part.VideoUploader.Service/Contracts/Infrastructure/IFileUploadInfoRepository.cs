using Part.VideoUploader.Domain;

namespace Part.VideoUploader.Service.Contracts.Infrastructure;

public interface IFileUploadInfoRepository
{
    Task AddAsync(FileUploadInfo fileUploadInfo);
    Task UpdateAsync(FileUploadInfo fileUploadInfo);
    Task<IEnumerable<FileUploadInfo>> GetFileUploadsByUserAsync(string userId);
}