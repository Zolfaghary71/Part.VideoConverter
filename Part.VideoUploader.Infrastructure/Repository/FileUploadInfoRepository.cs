using Microsoft.EntityFrameworkCore;
using Part.VideoUploader.Domain;
using Part.VideoUploader.Infrastructure.Db;
using Part.VideoUploader.Service.Contracts.Infrastructure;

namespace Part.VideoUploader.Infrastructure.Repository;


public class FileUploadInfoRepository : IFileUploadInfoRepository
{
    private readonly VideoUploaderDbContext _context;

    public FileUploadInfoRepository(VideoUploaderDbContext context)
    {
        _context = context;
    }

    public async Task AddAsync(FileUploadInfo fileUploadInfo)
    {
        await _context.FileUploadInfos.AddAsync(fileUploadInfo);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(FileUploadInfo fileUploadInfo)
    {
        _context.FileUploadInfos.Update(fileUploadInfo);
        await _context.SaveChangesAsync();
    }

    public async Task<IEnumerable<FileUploadInfo>> GetFileUploadsByUserAsync(string userId)
    {
        return await _context.FileUploadInfos
            .Where(f => f.UserId == userId)
            .ToListAsync();
    }
}
