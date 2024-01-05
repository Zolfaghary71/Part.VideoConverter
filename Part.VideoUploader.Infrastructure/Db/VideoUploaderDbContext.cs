using Part.VideoUploader.Domain;

namespace Part.VideoUploader.Infrastructure.Db;

using Microsoft.EntityFrameworkCore;

public class VideoUploaderDbContext : DbContext
{
    public VideoUploaderDbContext(DbContextOptions<VideoUploaderDbContext> options)
        : base(options)
    {
    }

    public DbSet<FileUploadInfo> FileUploadInfos { get; set; }

}
