namespace SL.Domain.VModels.Post
{
    public class PostMediaVModel
    {
        public long Id { get; set; }
        public long FileId { get; set; }
        public string FileUrl { get; set; } = null!;
        public string? ThumbnailUrl { get; set; }
        public string? MediaType { get; set; } // Image, Video
        public int DisplayOrder { get; set; }
    }
}

