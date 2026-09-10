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
    public class RelationshipService : Globals, IRelationshipService
    {
        private readonly SnapLifeContext _context;
        private readonly UserManager<AspNetUsers> _userManager;

        public RelationshipService(
            SnapLifeContext context,
            UserManager<AspNetUsers> userManager,
            IHttpContextAccessor contextAccessor) : base(contextAccessor)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<RelationshipActionResponse> ToggleFollowAsync(string targetUserId)
        {
            var currentUserId = GlobalUserId;
            if (string.IsNullOrEmpty(currentUserId))
            {
                return new RelationshipActionResponse { IsSuccess = false, Message = Strings.Messages.UserNotAuthenticated };
            }

            if (currentUserId == targetUserId)
            {
                return new RelationshipActionResponse { IsSuccess = false, Message = "You cannot follow yourself." };
            }

            var targetUser = await _userManager.FindByIdAsync(targetUserId);
            var currentUser = await _userManager.FindByIdAsync(currentUserId);
            if (targetUser == null || currentUser == null)
            {
                return new RelationshipActionResponse { IsSuccess = false, Message = Strings.Messages.NotFound };
            }

            var existingFollow = await _context.UserRelationships
                .FirstOrDefaultAsync(r => r.SourceUserId == currentUserId
                                       && r.TargetUserId == targetUserId
                                       && r.Type == RelationshipType.Follow);

            if (existingFollow != null)
            {
                // Đang follow -> Bỏ follow (Unfollow)
                _context.UserRelationships.Remove(existingFollow);

                currentUser.FollowingCount = Math.Max(0, currentUser.FollowingCount - 1);
                targetUser.FollowersCount = Math.Max(0, targetUser.FollowersCount - 1);

                await _context.SaveChangesAsync();

                return new RelationshipActionResponse
                {
                    IsSuccess = true,
                    Message = $"Unfollowed {targetUser.UserName} successfully.",
                    Type = RelationshipType.Follow,
                    Status = null
                };
            }
            else
            {
                // Chưa follow -> Tiến hành Follow
                var newFollow = new UserRelationship
                {
                    SourceUserId = currentUserId,
                    TargetUserId = targetUserId,
                    Type = RelationshipType.Follow,
                    Status = RelationshipStatus.Accepted,
                    CreatedDate = DateTime.UtcNow,
                    CreatedBy = currentUserId
                };

                _context.UserRelationships.Add(newFollow);

                currentUser.FollowingCount++;
                targetUser.FollowersCount++;

                // Thêm thông báo
                var notification = new Notification
                {
                    ReceiverId = targetUserId,
                    ActorId = currentUserId,
                    Type = NotificationType.Followed,
                    Message = $"{currentUser.FullName} started following you.",
                    CreatedDate = DateTime.UtcNow,
                    CreatedBy = currentUserId
                };
                _context.Notifications.Add(notification);

                await _context.SaveChangesAsync();

                return new RelationshipActionResponse
                {
                    IsSuccess = true,
                    Message = $"Followed {targetUser.UserName} successfully.",
                    Type = RelationshipType.Follow,
                    Status = RelationshipStatus.Accepted
                };
            }
        }

        public async Task<RelationshipActionResponse> SendFriendRequestAsync(string targetUserId)
        {
            var currentUserId = GlobalUserId;
            if (string.IsNullOrEmpty(currentUserId))
            {
                return new RelationshipActionResponse { IsSuccess = false, Message = Strings.Messages.UserNotAuthenticated };
            }

            if (currentUserId == targetUserId)
            {
                return new RelationshipActionResponse { IsSuccess = false, Message = "You cannot friend yourself." };
            }

            var targetUser = await _userManager.FindByIdAsync(targetUserId);
            if (targetUser == null)
            {
                return new RelationshipActionResponse { IsSuccess = false, Message = Strings.Messages.NotFound };
            }

            // Kiểm tra đã là bạn bè chưa
            var isFriend = await _context.UserRelationships.AnyAsync(r =>
                ((r.SourceUserId == currentUserId && r.TargetUserId == targetUserId) ||
                 (r.SourceUserId == targetUserId && r.TargetUserId == currentUserId)) &&
                r.Type == RelationshipType.Friend && r.Status == RelationshipStatus.Accepted);

            if (isFriend)
            {
                return new RelationshipActionResponse { IsSuccess = false, Message = "You are already friends with this user." };
            }

            // Kiểm tra đã có lời mời kết bạn chưa
            var existingRequest = await _context.UserRelationships.FirstOrDefaultAsync(r =>
                r.SourceUserId == currentUserId && r.TargetUserId == targetUserId &&
                r.Type == RelationshipType.FriendRequest && r.Status == RelationshipStatus.Pending);

            if (existingRequest != null)
            {
                return new RelationshipActionResponse { IsSuccess = false, Message = "Friend request already sent." };
            }

            var request = new UserRelationship
            {
                SourceUserId = currentUserId,
                TargetUserId = targetUserId,
                Type = RelationshipType.FriendRequest,
                Status = RelationshipStatus.Pending,
                CreatedDate = DateTime.UtcNow,
                CreatedBy = currentUserId
            };

            _context.UserRelationships.Add(request);
            await _context.SaveChangesAsync();

            return new RelationshipActionResponse
            {
                IsSuccess = true,
                Message = "Friend request sent successfully.",
                Type = RelationshipType.FriendRequest,
                Status = RelationshipStatus.Pending
            };
        }

        public async Task<RelationshipActionResponse> AcceptFriendRequestAsync(long requestId)
        {
            var currentUserId = GlobalUserId;
            var request = await _context.UserRelationships.FindAsync(requestId);

            if (request == null || request.TargetUserId != currentUserId || request.Type != RelationshipType.FriendRequest)
            {
                return new RelationshipActionResponse { IsSuccess = false, Message = "Friend request not found or unauthorized." };
            }

            // Chuyển quan hệ thành Friend
            request.Type = RelationshipType.Friend;
            request.Status = RelationshipStatus.Accepted;
            request.UpdatedDate = DateTime.UtcNow;
            request.UpdatedBy = currentUserId;

            await _context.SaveChangesAsync();

            return new RelationshipActionResponse
            {
                IsSuccess = true,
                Message = "Friend request accepted.",
                Type = RelationshipType.Friend,
                Status = RelationshipStatus.Accepted
            };
        }

        public async Task<RelationshipActionResponse> DeclineFriendRequestAsync(long requestId)
        {
            var currentUserId = GlobalUserId;
            var request = await _context.UserRelationships.FindAsync(requestId);

            if (request == null || request.TargetUserId != currentUserId || request.Type != RelationshipType.FriendRequest)
            {
                return new RelationshipActionResponse { IsSuccess = false, Message = "Friend request not found or unauthorized." };
            }

            _context.UserRelationships.Remove(request);
            await _context.SaveChangesAsync();

            return new RelationshipActionResponse
            {
                IsSuccess = true,
                Message = "Friend request declined."
            };
        }

        public async Task<RelationshipActionResponse> UnfriendAsync(string targetUserId)
        {
            var currentUserId = GlobalUserId;
            var friendRel = await _context.UserRelationships.FirstOrDefaultAsync(r =>
                ((r.SourceUserId == currentUserId && r.TargetUserId == targetUserId) ||
                 (r.SourceUserId == targetUserId && r.TargetUserId == currentUserId)) &&
                r.Type == RelationshipType.Friend);

            if (friendRel == null)
            {
                return new RelationshipActionResponse { IsSuccess = false, Message = "Not friends with this user." };
            }

            _context.UserRelationships.Remove(friendRel);
            await _context.SaveChangesAsync();

            return new RelationshipActionResponse
            {
                IsSuccess = true,
                Message = "Unfriended successfully."
            };
        }

        public async Task<RelationshipActionResponse> ToggleBlockAsync(string targetUserId)
        {
            var currentUserId = GlobalUserId;
            if (currentUserId == targetUserId)
            {
                return new RelationshipActionResponse { IsSuccess = false, Message = "Cannot block yourself." };
            }

            var blockRel = await _context.UserRelationships.FirstOrDefaultAsync(r =>
                r.SourceUserId == currentUserId && r.TargetUserId == targetUserId && r.Type == RelationshipType.Block);

            if (blockRel != null)
            {
                _context.UserRelationships.Remove(blockRel);
                await _context.SaveChangesAsync();
                return new RelationshipActionResponse { IsSuccess = true, Message = "User unblocked successfully." };
            }
            else
            {
                // Xóa mọi quan hệ follow / friend trước đó nếu block
                var allOldRelations = await _context.UserRelationships.Where(r =>
                    (r.SourceUserId == currentUserId && r.TargetUserId == targetUserId) ||
                    (r.SourceUserId == targetUserId && r.TargetUserId == currentUserId)).ToListAsync();

                _context.UserRelationships.RemoveRange(allOldRelations);

                _context.UserRelationships.Add(new UserRelationship
                {
                    SourceUserId = currentUserId,
                    TargetUserId = targetUserId,
                    Type = RelationshipType.Block,
                    Status = RelationshipStatus.Accepted,
                    CreatedDate = DateTime.UtcNow,
                    CreatedBy = currentUserId
                });

                await _context.SaveChangesAsync();
                return new RelationshipActionResponse { IsSuccess = true, Message = "User blocked successfully." };
            }
        }

        public async Task<PaginationModel<UserSummaryVModel>> GetFollowersAsync(string userId, RelationshipFilterVModel filter)
        {
            var query = _context.UserRelationships
                .AsNoTracking()
                .Where(r => r.TargetUserId == userId && r.Type == RelationshipType.Follow)
                .Select(r => r.SourceUser);

            return await PaginateUsersAsync(query, filter);
        }

        public async Task<PaginationModel<UserSummaryVModel>> GetFollowingAsync(string userId, RelationshipFilterVModel filter)
        {
            var query = _context.UserRelationships
                .AsNoTracking()
                .Where(r => r.SourceUserId == userId && r.Type == RelationshipType.Follow)
                .Select(r => r.TargetUser);

            return await PaginateUsersAsync(query, filter);
        }

        public async Task<PaginationModel<UserSummaryVModel>> GetFriendsAsync(string userId, RelationshipFilterVModel filter)
        {
            var friendsSource = _context.UserRelationships
                .AsNoTracking()
                .Where(r => r.TargetUserId == userId && r.Type == RelationshipType.Friend && r.Status == RelationshipStatus.Accepted)
                .Select(r => r.SourceUser);

            var friendsTarget = _context.UserRelationships
                .AsNoTracking()
                .Where(r => r.SourceUserId == userId && r.Type == RelationshipType.Friend && r.Status == RelationshipStatus.Accepted)
                .Select(r => r.TargetUser);

            var query = friendsSource.Union(friendsTarget);

            return await PaginateUsersAsync(query, filter);
        }

        public async Task<List<FriendRequestItemVModel>> GetPendingFriendRequestsAsync()
        {
            var currentUserId = GlobalUserId;
            return await _context.UserRelationships
                .AsNoTracking()
                .Where(r => r.TargetUserId == currentUserId && r.Type == RelationshipType.FriendRequest && r.Status == RelationshipStatus.Pending)
                .OrderByDescending(r => r.CreatedDate)
                .Select(r => new FriendRequestItemVModel
                {
                    RequestId = r.Id,
                    SenderId = r.SourceUserId,
                    SenderUserName = r.SourceUser.UserName,
                    SenderFullName = r.SourceUser.FullName,
                    SenderAvatarUrl = r.SourceUser.AvatarUrl,
                    CreatedDate = r.CreatedDate
                })
                .ToListAsync();
        }

        private async Task<PaginationModel<UserSummaryVModel>> PaginateUsersAsync(IQueryable<AspNetUsers> query, RelationshipFilterVModel filter)
        {
            var total = await query.CountAsync();
            var page = Math.Max(1, filter.PageNumber);
            var size = Math.Clamp(filter.PageSize, 1, 100);

            var users = await query
                .OrderByDescending(u => u.CreatedDate)
                .Skip((page - 1) * size)
                .Take(size)
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

            return new PaginationModel<UserSummaryVModel>
            {
                TotalRecords = total,
                Records = users
            };
        }
    }
}

