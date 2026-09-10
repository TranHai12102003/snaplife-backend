using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using SL.Domain.Common.Constants;
using SL.Domain.IServices;
using SL.Domain.VModels.Reaction;
using SL.Domain.VModels.User;
using SL.Infrastructures.EntityFramework;
using SL.Infrastructures.EntityFramework.Entities;
using SL.Services.Mappings;

namespace SL.Services.Services
{
    public class ReactionService : Globals, IReactionService
    {
        private readonly SnapLifeContext _context;

        public ReactionService(
            SnapLifeContext context,
            IHttpContextAccessor contextAccessor) : base(contextAccessor)
        {
            _context = context;
        }

        public async Task<ReactionActionResponse> ReactToPostAsync(long postId, ReactionRequest request)
        {
            var currentUserId = GlobalUserId;
            if (string.IsNullOrEmpty(currentUserId))
            {
                return new ReactionActionResponse { IsSuccess = false, Message = Strings.Messages.UserNotAuthenticated };
            }

            var post = await _context.Posts.FindAsync(postId);
            if (post == null || post.IsActive != true)
            {
                return new ReactionActionResponse { IsSuccess = false, Message = "Snap not found." };
            }

            var existingReaction = await _context.Reactions
                .FirstOrDefaultAsync(r => r.UserId == currentUserId
                                       && r.TargetType == ReactionTargetType.Post
                                       && r.TargetId == postId);

            if (existingReaction != null)
            {
                // Nếu bấm lại cùng một reaction -> Bỏ thả biểu cảm (Toggle off)
                if (existingReaction.Type == request.Type)
                {
                    _context.Reactions.Remove(existingReaction);
                    if (post.LikeCount > 0) post.LikeCount--;
                    await _context.SaveChangesAsync();

                    return new ReactionActionResponse
                    {
                        IsSuccess = true,
                        IsReacted = false,
                        CurrentReaction = null,
                        TotalReactions = post.LikeCount,
                        Message = "Reaction removed."
                    };
                }
                else
                {
                    // Đổi sang reaction khác (ví dụ từ Like sang Love)
                    existingReaction.Type = request.Type;
                    existingReaction.UpdatedDate = DateTime.UtcNow;
                    existingReaction.UpdatedBy = currentUserId;
                    await _context.SaveChangesAsync();

                    return new ReactionActionResponse
                    {
                        IsSuccess = true,
                        IsReacted = true,
                        CurrentReaction = request.Type,
                        TotalReactions = post.LikeCount,
                        Message = "Reaction updated."
                    };
                }
            }

            // Tạo mới reaction
            var newReaction = new Reaction
            {
                UserId = currentUserId,
                TargetType = ReactionTargetType.Post,
                TargetId = postId,
                Type = request.Type,
                CreatedDate = DateTime.UtcNow,
                CreatedBy = currentUserId,
                IsActive = true
            };

            _context.Reactions.Add(newReaction);
            post.LikeCount++;

            // Thêm thông báo cho chủ bài viết (nếu không phải tự react bài của mình)
            if (post.UserId != currentUserId)
            {
                var currentUser = await _context.Users.FindAsync(currentUserId);
                var notification = new Notification
                {
                    ReceiverId = post.UserId,
                    ActorId = currentUserId,
                    Type = NotificationType.LikedPost,
                    Message = $"{currentUser?.FullName ?? "A friend"} reacted {request.Type} to your Snap.",
                    CreatedDate = DateTime.UtcNow,
                    CreatedBy = currentUserId
                };
                _context.Notifications.Add(notification);
            }

            await _context.SaveChangesAsync();

            return new ReactionActionResponse
            {
                IsSuccess = true,
                IsReacted = true,
                CurrentReaction = request.Type,
                TotalReactions = post.LikeCount,
                Message = "Reaction added."
            };
        }

        public async Task<List<ReactionItemVModel>> GetPostReactionsAsync(long postId)
        {
            var reactions = await _context.Reactions
                .Include(r => r.User)
                .Where(r => r.TargetType == ReactionTargetType.Post
                         && r.TargetId == postId
                         && r.IsActive == true)
                .OrderByDescending(r => r.CreatedDate)
                .ToListAsync();

            return reactions.Select(ReactionMappings.EntityToItemVModel).ToList();
        }
    }
}
