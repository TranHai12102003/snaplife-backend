using SL.Domain.Common.Models;
using SL.Domain.VModels.SysVModels;

namespace SL.Domain.IServices.ISysServices
{
    public interface IAuthService
    {
        Task<LoginResponse> Login(LoginVModel model);
        Task<RegisterResponse> Register(RegisterVModel model);
        Task<ResponseResult> Me();
        Task<ResponseResult> UpdateAvatar(long fileId);
        Task<ResponseResult> UpdateCoverImage(long fileId);
    }
}

