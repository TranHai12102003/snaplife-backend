using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SL.Domain.Common.Constants;
using SL.Domain.Common.Models;
using SL.Domain.IServices;
using SL.Domain.VModels.User;
using SL.Infrastructures.EntityFramework;
using SL.Infrastructures.EntityFramework.Entities;
using SL.Infrastructures.EntityFramework.Entities.SysEntities;

namespace SL.Services.Services
{
    public class UserService : Globals, IUserService
    {
        private readonly UserManager<AspNetUsers> _userManager;
        private readonly SnapLifeContext _context;

        public UserService(
            UserManager<AspNetUsers> userManager,
            SnapLifeContext context,
            IHttpContextAccessor contextAccessor) : base(contextAccessor)
        {
            _userManager = userManager;
            _context = context;
        }

        public async Task<UserProfileVModel?> GetProfileAsync(string userIdOrUserName)
        {
            if (string.IsNullOrWhiteSpace(userIdOrUserName)) return null;

            var targetUser = await _userManager.FindByIdAsync(userIdOrUserName)
                             ?? await _userManager.FindByNameAsync(userIdOrUserName);

            if (targetUser == null || targetUser.IsActive != true) return null;

            var profile = new UserProfileVModel
            {
                Id = targetUser.Id,
                UserName = targetUser.UserName,
                Email = targetUser.Email,
                FirstName = targetUser.FirstName,
                LastName = targetUser.LastName,
                FullName = targetUser.FullName,
                AvatarUrl = targetUser.AvatarUrl,
                CoverImageUrl = targetUser.CoverImageUrl,
                Sex = targetUser.Sex,
                Birthday = targetUser.Birthday,
                Address = targetUser.Address,
                Bio = targetUser.Bio,
                IsPrivate = targetUser.IsPrivate,
                IsVerified = targetUser.IsVerified,
                FollowersCount = targetUser.FollowersCount,
                FollowingCount = targetUser.FollowingCount,
                FriendsCount = await _context.UserRelationships
                    .CountAsync(r => (r.SourceUserId == targetUser.Id || r.TargetUserId == targetUser.Id)
                                  && r.Type == RelationshipType.Friend
                                  && r.Status == RelationshipStatus.Accepted
                                  && r.IsActive == true),
                PostsCount = targetUser.PostsCount,
                CreatedDate = targetUser.CreatedDate
            };

            // Nếu người gọi đã đăng nhập, tính toán trạng thái quan hệ
            var currentUserId = GlobalUserId;
            if (!string.IsNullOrEmpty(currentUserId) && currentUserId != targetUser.Id)
            {
                var relationships = await _context.UserRelationships
                    .Where(r => (r.SourceUserId == currentUserId && r.TargetUserId == targetUser.Id)
                             || (r.SourceUserId == targetUser.Id && r.TargetUserId == currentUserId))
                    .ToListAsync();

                profile.IsFollowing = relationships.Any(r => r.SourceUserId == currentUserId
                                                          && r.TargetUserId == targetUser.Id
                                                          && r.Type == RelationshipType.Follow);

                profile.IsFollowedBy = relationships.Any(r => r.SourceUserId == targetUser.Id
                                                           && r.TargetUserId == currentUserId
                                                           && r.Type == RelationshipType.Follow);

                profile.IsFriend = relationships.Any(r => r.Type == RelationshipType.Friend
                                                       && r.Status == RelationshipStatus.Accepted);

                profile.HasSentFriendRequest = relationships.Any(r => r.SourceUserId == currentUserId
                                                                   && r.TargetUserId == targetUser.Id
                                                                   && r.Type == RelationshipType.FriendRequest
                                                                   && r.Status == RelationshipStatus.Pending);

                profile.HasReceivedFriendRequest = relationships.Any(r => r.SourceUserId == targetUser.Id
                                                                       && r.TargetUserId == currentUserId
                                                                       && r.Type == RelationshipType.FriendRequest
                                                                       && r.Status == RelationshipStatus.Pending);

                profile.IsBlocked = relationships.Any(r => r.SourceUserId == currentUserId
                                                        && r.TargetUserId == targetUser.Id
                                                        && r.Type == RelationshipType.Block);
            }

            return profile;
        }

        public async Task<ResponseResult> UpdateProfileAsync(UpdateProfileRequest request)
        {
            if (string.IsNullOrEmpty(GlobalUserId))
            {
                return new ErrorResponseResult(Strings.Messages.UserNotAuthenticated);
            }

            var user = await _userManager.FindByIdAsync(GlobalUserId);
            if (user == null)
            {
                return new ErrorResponseResult(Strings.Messages.NotFound);
            }

            if (request.FirstName != null) user.FirstName = request.FirstName.Trim();
            if (request.LastName != null) user.LastName = request.LastName.Trim();
            if (request.Bio != null) user.Bio = request.Bio.Trim();
            if (request.Address != null) user.Address = request.Address.Trim();
            if (request.Sex.HasValue) user.Sex = request.Sex.Value;
            if (request.Birthday.HasValue) user.Birthday = request.Birthday.Value;
            if (request.IsPrivate.HasValue) user.IsPrivate = request.IsPrivate.Value;

            user.UpdatedDate = DateTime.UtcNow;
            user.UpdatedBy = GlobalUserId;

            var result = await _userManager.UpdateAsync(user);
            if (!result.Succeeded)
            {
                return new ErrorResponseResult("Failed to update profile.");
            }

            return new SuccessResponseResult(await GetProfileAsync(user.Id), "Profile updated successfully.");
        }

        public async Task<PaginationModel<UserSummaryVModel>> SearchUsersAsync(UserSearchFilterVModel filter)
        {
            var query = _context.Users.AsNoTracking().Where(u => u.IsActive == true);

            if (!string.IsNullOrWhiteSpace(filter.Keyword))
            {
                var kw = filter.Keyword.Trim().ToLower();
                query = query.Where(u => (u.UserName != null && u.UserName.ToLower().Contains(kw))
                                      || (u.FirstName != null && u.FirstName.ToLower().Contains(kw))
                                      || (u.LastName != null && u.LastName.ToLower().Contains(kw)));
            }

            var total = await query.CountAsync();
            var pageNumber = Math.Max(1, filter.PageNumber);
            var pageSize = Math.Clamp(filter.PageSize, 1, 100);

            var currentUserId = GlobalUserId;
            var followings = new HashSet<string>();
            if (!string.IsNullOrEmpty(currentUserId))
            {
                followings = (await _context.UserRelationships
                    .Where(r => r.SourceUserId == currentUserId && r.Type == RelationshipType.Follow)
                    .Select(r => r.TargetUserId)
                    .ToListAsync()).ToHashSet();
            }

            var users = await query
                .OrderByDescending(u => u.FollowersCount)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .Select(u => new UserSummaryVModel
                {
                    Id = u.Id,
                    UserName = u.UserName,
                    FullName = u.FullName,
                    AvatarUrl = u.AvatarUrl,
                    Bio = u.Bio,
                    IsVerified = u.IsVerified
                })
                .ToListAsync();

            foreach (var user in users)
            {
                user.IsFollowing = followings.Contains(user.Id);
            }

            return new PaginationModel<UserSummaryVModel>
            {
                TotalRecords = total,
                Records = users
            };
        }
    }
}

