using System.IO.Enumeration;
using System.Security.Cryptography;

namespace Part.VideoUploader.Domain;

public class VideoFile
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Type { get; set; }
    public string Path { get; set; }
    public DateTime DateCreated { get; set; }
}