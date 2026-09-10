using System.ComponentModel.DataAnnotations.Schema;

namespace SL.Infrastructures.EntityFramework.Entities.SysEntities
{
    [Table("MediaFiles")]
    public class SysFile : BaseEntity
    {
        public string FileName { get; set; } = null!;
        public string FilePath { get; set; } = null!;
        public string? FileType { get; set; } // Image, Video, Audio, Document
        public string? MimeType { get; set; }
        public long? FileSize { get; set; }
        public string? StorageProvider { get; set; } = "Local"; // Local, S3, Cloudinary...
        public int? Width { get; set; }
        public int? Height { get; set; }
        public int? Duration { get; set; } // Duration in seconds (cho video/audio)
    }
}

