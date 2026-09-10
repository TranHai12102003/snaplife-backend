using SL.Domain.Common.Models;
using SL.Domain.VModels.User;

namespace SL.Domain.IServices
{
    public interface IRelationshipService
    {
        Task<RelationshipActionResponse> ToggleFollowAsync(string targetUserId);
        Task<RelationshipActionResponse> SendFriendRequestAsync(string targetUserId);
        Task<RelationshipActionResponse> AcceptFriendRequestAsync(long requestId);
        Task<RelationshipActionResponse> DeclineFriendRequestAsync(long requestId);
        Task<RelationshipActionResponse> UnfriendAsync(string targetUserId);
        Task<RelationshipActionResponse> ToggleBlockAsync(string targetUserId);
        Task<PaginationModel<UserSummaryVModel>> GetFollowersAsync(string userId, RelationshipFilterVModel filter);
        Task<PaginationModel<UserSummaryVModel>> GetFollowingAsync(string userId, RelationshipFilterVModel filter);
        Task<PaginationModel<UserSummaryVModel>> GetFriendsAsync(string userId, RelationshipFilterVModel filter);
        Task<List<FriendRequestItemVModel>> GetPendingFriendRequestsAsync();
    }
}

