using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SL.Domain.Common.Constants;
using SL.Domain.IServices;
using SL.Domain.VModels.Media;

namespace SL.WebApi.Controllers
{
    [Route(Strings.ActionRoute)]
    [ApiController]
    public class FileController : ControllerBase
    {
        private readonly IFileService _fileService;

        public FileController(IFileService fileService)
        {
            _fileService = fileService;
        }

        [HttpPost]
        [Authorize]
        [Consumes("multipart/form-data")]
        public async Task<ActionResult<UploadFileResponse>> Upload([FromForm] UploadFileRequest request)
        {
            try
            {
                var result = await _fileService.UploadFileAsync(request.File, request.SubFolder);
                return Ok(result);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while uploading file: " + ex.Message });
            }
        }

        [HttpPost]
        [Authorize]
        [Consumes("multipart/form-data")]
        public async Task<ActionResult<UploadMultipleFilesResponse>> UploadMultiple([FromForm] UploadMultipleFilesRequest request)
        {
            try
            {
                var result = await _fileService.UploadMultipleFilesAsync(request.Files, request.SubFolder);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while uploading files: " + ex.Message });
            }
        }

        [HttpDelete("{fileId}")]
        [Authorize]
        public async Task<IActionResult> Delete(long fileId)
        {
            var success = await _fileService.DeleteFileAsync(fileId);
            if (!success)
            {
                return NotFound(new { message = "File not found." });
            }

            return Ok(new { message = "File deleted successfully." });
        }
    }
}
