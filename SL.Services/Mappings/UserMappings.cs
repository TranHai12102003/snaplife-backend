using SL.Domain.VModels.User;
using SL.Infrastructures.EntityFramework.Entities.SysEntities;

namespace SL.Services.Mappings
{
    public static class UserMappings
    {
        public static UserSummaryVModel EntityToSummary(AspNetUsers entity, bool isFriend = false, bool isFollowing = false)
        {
            return new UserSummaryVModel
            {
                Id = entity.Id,
                UserName = entity.UserName,
                FullName = entity.FullName,
                AvatarUrl = entity.AvatarUrl,
                Bio = entity.Bio,
                IsVerified = entity.IsVerified,
                IsFriend = isFriend,
                IsFollowing = isFollowing
            };
        }

        public static UserProfileVModel EntityToProfile(
            AspNetUsers entity,
            int friendsCount,
            bool isFollowing = false,
            bool isFollowedBy = false,
            bool isFriend = false,
            bool hasSentFriendRequest = false,
            bool hasReceivedFriendRequest = false,
            bool isBlocked = false)
        {
            return new UserProfileVModel
            {
                Id = entity.Id,
                UserName = entity.UserName,
                Email = entity.Email,
                FirstName = entity.FirstName,
                LastName = entity.LastName,
                FullName = entity.FullName,
                AvatarUrl = entity.AvatarUrl,
                CoverImageUrl = entity.CoverImageUrl,
                Sex = entity.Sex,
                Birthday = entity.Birthday,
                Address = entity.Address,
                Bio = entity.Bio,
                IsPrivate = entity.IsPrivate,
                IsVerified = entity.IsVerified,
                FollowersCount = entity.FollowersCount,
                FollowingCount = entity.FollowingCount,
                FriendsCount = friendsCount,
                PostsCount = entity.PostsCount,
                CreatedDate = entity.CreatedDate,
                IsFollowing = isFollowing,
                IsFollowedBy = isFollowedBy,
                IsFriend = isFriend,
                HasSentFriendRequest = hasSentFriendRequest,
                HasReceivedFriendRequest = hasReceivedFriendRequest,
                IsBlocked = isBlocked
            };
        }
    }
}

