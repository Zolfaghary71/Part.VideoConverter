namespace Part.VideoUploader.Domain;

using System;
using System.ComponentModel.DataAnnotations;

public class FileUploadInfo
{
    [Key]
    public Guid Id { get; set; }
    public string FileName { get; set; }
    public string UserId { get; set; }
    public long Size { get; set; }
    public DateTime UploadStartTime { get; set; }
    public DateTime? UploadEndTime { get; set; }
    public string Status { get; set; }
}
