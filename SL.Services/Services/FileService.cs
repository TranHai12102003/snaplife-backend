using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using SL.Domain.Common.Constants;
using SL.Domain.IServices;
using SL.Domain.VModels.Media;
using SL.Infrastructures.EntityFramework;
using SL.Infrastructures.EntityFramework.Entities.SysEntities;

namespace SL.Services.Services
{
    public class FileService : Globals, IFileService
    {
        private readonly SnapLifeContext _context;
        private readonly IWebHostEnvironment _env;
        private readonly IHttpContextAccessor _httpContextAccessor;

        // Các định dạng file hợp lệ
        private static readonly HashSet<string> AllowedImageExtensions = new(StringComparer.OrdinalIgnoreCase)
        {
            ".jpg", ".jpeg", ".png", ".webp", ".gif"
        };

        private static readonly HashSet<string> AllowedVideoExtensions = new(StringComparer.OrdinalIgnoreCase)
        {
            ".mp4", ".mov", ".webm"
        };

        private static readonly HashSet<string> DisallowedExtensions = new(StringComparer.OrdinalIgnoreCase)
        {
            ".exe", ".dll", ".bat", ".cmd", ".sh", ".php", ".asp", ".aspx", ".jsp", ".js", ".vbs", ".ps1"
        };

        // Giới hạn dung lượng: Ảnh 15 MB, Video 100 MB
        private const long MaxImageSize = 15 * 1024 * 1024;
        private const long MaxVideoSize = 100 * 1024 * 1024;

        public FileService(
            SnapLifeContext context,
            IWebHostEnvironment env,
            IHttpContextAccessor httpContextAccessor) : base(httpContextAccessor)
        {
            _context = context;
            _env = env;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<UploadFileResponse> UploadFileAsync(IFormFile file, string? subFolder = null)
        {
            if (file == null || file.Length == 0)
            {
                throw new ArgumentException("File is empty or not provided.");
            }

            var extension = Path.GetExtension(file.FileName).ToLowerInvariant();

            if (DisallowedExtensions.Contains(extension))
            {
                throw new InvalidOperationException($"File extension '{extension}' is strictly forbidden.");
            }

            string fileType;
            if (AllowedImageExtensions.Contains(extension))
            {
                if (file.Length > MaxImageSize)
                {
                    throw new InvalidOperationException("Image file size exceeds maximum limit of 15 MB.");
                }
                fileType = "Image";
            }
            else if (AllowedVideoExtensions.Contains(extension))
            {
                if (file.Length > MaxVideoSize)
                {
                    throw new InvalidOperationException("Video file size exceeds maximum limit of 100 MB.");
                }
                fileType = "Video";
            }
            else
            {
                throw new InvalidOperationException($"Unsupported file format: '{extension}'. Only standard images and videos are supported.");
            }

            // Tạo thư mục lưu trữ theo thời gian: uploads/{type}/{yyyy}/{MM}/
            var folderName = string.IsNullOrWhiteSpace(subFolder) ? fileType.ToLowerInvariant() : subFolder.Trim();
            var datePath = Path.Combine("uploads", folderName, DateTime.UtcNow.ToString("yyyy"), DateTime.UtcNow.ToString("MM"));
            
            var webRoot = _env.WebRootPath;
            if (string.IsNullOrEmpty(webRoot))
            {
                webRoot = Path.Combine(_env.ContentRootPath, "wwwroot");
            }

            var absoluteTargetDir = Path.Combine(webRoot, datePath);
            if (!Directory.Exists(absoluteTargetDir))
            {
                Directory.CreateDirectory(absoluteTargetDir);
            }

            // Sinh tên file ngẫu nhiên tránh trùng lặp
            var uniqueFileName = $"{Guid.NewGuid():N}_{DateTime.UtcNow.Ticks}{extension}";
            var absoluteFilePath = Path.Combine(absoluteTargetDir, uniqueFileName);

            using (var stream = new FileStream(absoluteFilePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            // Đường dẫn tương đối phục vụ URL tĩnh
            var relativePath = "/" + Path.Combine(datePath, uniqueFileName).Replace("\\", "/");

            var sysFile = new SysFile
            {
                FileName = file.FileName,
                FilePath = relativePath,
                FileType = fileType,
                MimeType = file.ContentType,
                FileSize = file.Length,
                StorageProvider = "Local",
                CreatedBy = GlobalUserId,
                CreatedDate = DateTime.UtcNow,
                IsActive = true
            };

            _context.SysFiles.Add(sysFile);
            await _context.SaveChangesAsync();

            var request = _httpContextAccessor.HttpContext?.Request;
            var baseUrl = request != null ? $"{request.Scheme}://{request.Host}" : "";
            var fullUrl = $"{baseUrl}{relativePath}";

            return new UploadFileResponse
            {
                FileId = sysFile.Id,
                FileName = sysFile.FileName,
                FilePath = sysFile.FilePath,
                FileUrl = fullUrl,
                FileType = sysFile.FileType,
                MimeType = sysFile.MimeType,
                FileSize = sysFile.FileSize,
                CreatedDate = sysFile.CreatedDate
            };
        }

        public async Task<UploadMultipleFilesResponse> UploadMultipleFilesAsync(List<IFormFile> files, string? subFolder = null)
        {
            var response = new UploadMultipleFilesResponse();

            if (files == null || files.Count == 0)
            {
                response.IsSuccess = false;
                response.Message = "No files were uploaded.";
                return response;
            }

            foreach (var file in files)
            {
                try
                {
                    var uploaded = await UploadFileAsync(file, subFolder);
                    response.Files.Add(uploaded);
                }
                catch (Exception)
                {
                    // Tiếp tục các file khác nếu 1 file lỗi
                }
            }

            response.TotalUploaded = response.Files.Count;
            response.IsSuccess = response.TotalUploaded > 0;
            response.Message = $"Uploaded {response.TotalUploaded}/{files.Count} files successfully.";
            return response;
        }

        public async Task<bool> DeleteFileAsync(long fileId)
        {
            var sysFile = await _context.SysFiles.FindAsync(fileId);
            if (sysFile == null)
            {
                return false;
            }

            // Xóa file vật lý
            var webRoot = _env.WebRootPath ?? Path.Combine(_env.ContentRootPath, "wwwroot");
            var relativeClean = sysFile.FilePath.TrimStart('/').Replace("/", "\\");
            var absolutePath = Path.Combine(webRoot, relativeClean);

            if (File.Exists(absolutePath))
            {
                try
                {
                    File.Delete(absolutePath);
                }
                catch
                {
                    // Log ignore
                }
            }

            _context.SysFiles.Remove(sysFile);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
