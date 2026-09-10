using SL.Domain.Common.Models;
using SL.Domain.VModels.User;

namespace SL.Domain.IServices
{
    public interface IUserService
    {
        Task<UserProfileVModel?> GetProfileAsync(string userIdOrUserName);
        Task<ResponseResult> UpdateProfileAsync(UpdateProfileRequest request);
        Task<PaginationModel<UserSummaryVModel>> SearchUsersAsync(UserSearchFilterVModel filter);
    }
}

