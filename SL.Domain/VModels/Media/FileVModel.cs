namespace SL.Domain.VModels.Media
{
    public class UploadFileResponse
    {
        public long FileId { get; set; }
        public string FileName { get; set; } = null!;
        public string FilePath { get; set; } = null!;
        public string FileUrl { get; set; } = null!;
        public string? FileType { get; set; }
        public string? MimeType { get; set; }
        public long? FileSize { get; set; }
        public DateTime? CreatedDate { get; set; }
    }

    public class UploadMultipleFilesResponse
    {
        public bool IsSuccess { get; set; } = true;
        public string? Message { get; set; }
        public int TotalUploaded { get; set; }
        public List<UploadFileResponse> Files { get; set; } = new List<UploadFileResponse>();
    }

    public class UpdateAvatarRequest
    {
        public long FileId { get; set; }
    }

    public class UpdateCoverRequest
    {
        public long FileId { get; set; }
    }

    public class UploadFileRequest
    {
        public Microsoft.AspNetCore.Http.IFormFile File { get; set; } = null!;
        public string? SubFolder { get; set; }
    }

    public class UploadMultipleFilesRequest
    {
        public List<Microsoft.AspNetCore.Http.IFormFile> Files { get; set; } = new List<Microsoft.AspNetCore.Http.IFormFile>();
        public string? SubFolder { get; set; }
    }
}
