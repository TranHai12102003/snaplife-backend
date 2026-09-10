using Microsoft.AspNetCore.Http;
using SL.Domain.VModels.Media;

namespace SL.Domain.IServices
{
    public interface IFileService
    {
        Task<UploadFileResponse> UploadFileAsync(IFormFile file, string? subFolder = null);
        Task<UploadMultipleFilesResponse> UploadMultipleFilesAsync(List<IFormFile> files, string? subFolder = null);
        Task<bool> DeleteFileAsync(long fileId);
    }
}

