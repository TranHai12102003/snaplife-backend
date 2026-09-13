using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Linq.Expressions;
using SL.Domain.Common.Constants;
using SL.Domain.Common.Models;
using SL.Domain.IServices;
using SL.Domain.VModels.Post;
using SL.Domain.VModels.User;
using SL.Infrastructures.EntityFramework;
using SL.Infrastructures.EntityFramework.Entities;
using SL.Services.Helpers;
using SL.Services.Mappings;
using System.Text.RegularExpressions;

namespace SL.Services.Services
{
    public class PostService : Globals, IPostService
    {
        private readonly SnapLifeContext _context;
        private readonly IConfiguration _configuration;

        public PostService(
            SnapLifeContext context,
            IHttpContextAccessor contextAccessor,
            IConfiguration configuration) : base(contextAccessor)
        {
            _context = context;
            _configuration = configuration;
        }

        public async Task<ResponseResult> CreatePostAsync(PostCreateRequest request)
        {
            var currentUserId = GlobalUserId;
            if (string.IsNullOrEmpty(currentUserId))
            {
                return new ErrorResponseResult(Strings.Messages.UserNotAuthenticated);
            }

            var currentUser = await _context.Users.FindAsync(currentUserId);
            if (currentUser == null || currentUser.IsActive != true)
            {
                return new ErrorResponseResult("User account not found or inactive.");
            }

            var post = PostMappings.CreateRequestToEntity(request, currentUserId);

            _context.Posts.Add(post);
            await _context.SaveChangesAsync();

            // 1. Gắn ảnh/video đính kèm từ MediaFiles
            if (request.MediaFileIds != null && request.MediaFileIds.Count > 0)
            {
                var files = await _context.SysFiles
                    .Where(f => request.MediaFileIds.Contains(f.Id) && f.IsActive == true)
                    .ToListAsync();

                int order = 0;
                foreach (var fileId in request.MediaFileIds)
                {
                    var file = files.FirstOrDefault(f => f.Id == fileId);
                    if (file != null)
                    {
                        var postMedia = new PostMedia
                        {
                            PostId = post.Id,
                            FileId = file.Id,
                            DisplayOrder = order++,
                            MediaType = file.FileType,
                            CreatedDate = DateTime.UtcNow,
                            CreatedBy = currentUserId,
                            IsActive = true
                        };
                        _context.PostMedias.Add(postMedia);
                    }
                }
            }

            // 2. Trích xuất Hashtags tự động từ nội dung (Regex #\w+)
            await ProcessHashtagsAsync(post.Id, request.Content);

            // 3. Tăng tổng số bài viết của User
            currentUser.PostsCount++;

            await _context.SaveChangesAsync();

            return new SuccessResponseResult(new { PostId = post.Id }, "Snap created successfully.");
        }

        public async Task<ResponseResult> UpdatePostAsync(long postId, PostUpdateRequest request)
        {
            var currentUserId = GlobalUserId;
            var post = await _context.Posts
                .Include(p => p.PostHashtags)
                .FirstOrDefaultAsync(p => p.Id == postId && p.IsActive == true);

            if (post == null)
            {
                return new ErrorResponseResult("Snap not found.");
            }

            if (post.UserId != currentUserId)
            {
                return new ErrorResponseResult("You are not authorized to update this Snap.");
            }

            var oldContent = post.Content;
            post.Content = request.Content;
            post.LocationName = request.LocationName;
            post.Latitude = request.Latitude;
            post.Longitude = request.Longitude;
            post.Privacy = request.Privacy;
            post.IsPinned = request.IsPinned;
            post.IsExpense = request.IsExpense;
            post.Amount = request.IsExpense ? request.Amount : null;
            post.Currency = request.Currency ?? "VND";
            post.FoodName = request.IsExpense ? request.FoodName : null;
            post.ExpenseCategoryId = request.IsExpense ? request.ExpenseCategoryId : null;
            post.ShowAmountToFriends = request.ShowAmountToFriends;
            post.UpdatedDate = DateTime.UtcNow;
            post.UpdatedBy = currentUserId;

            if (oldContent != request.Content)
            {
                _context.PostHashtags.RemoveRange(post.PostHashtags);
                await ProcessHashtagsAsync(post.Id, request.Content);
            }

            await _context.SaveChangesAsync();

            return new SuccessResponseResult("Snap updated successfully.");
        }

        public async Task<ResponseResult> DeletePostAsync(long postId)
        {
            var currentUserId = GlobalUserId;
            var post = await _context.Posts.FirstOrDefaultAsync(p => p.Id == postId && p.IsActive == true);

            if (post == null)
            {
                return new ErrorResponseResult("Snap not found.");
            }

            if (post.UserId != currentUserId)
            {
                return new ErrorResponseResult("You are not authorized to delete this Snap.");
            }

            post.IsActive = false;
            post.UpdatedDate = DateTime.UtcNow;
            post.UpdatedBy = currentUserId;

            var user = await _context.Users.FindAsync(currentUserId);
            if (user != null && user.PostsCount > 0)
            {
                user.PostsCount--;
            }

            await _context.SaveChangesAsync();

            return new SuccessResponseResult("Snap deleted successfully.");
        }

        public async Task<ResponseResult> TogglePinPostAsync(long postId)
        {
            var currentUserId = GlobalUserId;
            var post = await _context.Posts.FirstOrDefaultAsync(p => p.Id == postId && p.IsActive == true);

            if (post == null)
            {
                return new ErrorResponseResult("Snap not found.");
            }

            if (post.UserId != currentUserId)
            {
                return new ErrorResponseResult("You are not authorized to pin this Snap.");
            }

            post.IsPinned = !post.IsPinned;
            post.UpdatedDate = DateTime.UtcNow;
            post.UpdatedBy = currentUserId;

            await _context.SaveChangesAsync();

            return new SuccessResponseResult(new { IsPinned = post.IsPinned }, post.IsPinned ? "Snap pinned." : "Snap unpinned.");
        }

        public async Task<PostDetailVModel?> GetPostByIdAsync(long postId)
        {
            var currentUserId = GlobalUserId;

            var post = await _context.Posts
                .Include(p => p.User)
                .Include(p => p.PostMedias.Where(pm => pm.IsActive == true).OrderBy(pm => pm.DisplayOrder))
                    .ThenInclude(pm => pm.File)
                .Include(p => p.PostHashtags)
                    .ThenInclude(ph => ph.Hashtag)
                .FirstOrDefaultAsync(p => p.Id == postId && p.IsActive == true);

            if (post == null) return null;

            // Kiểm tra phân quyền truy cập
            if (!string.IsNullOrEmpty(currentUserId) && currentUserId != post.UserId)
            {
                // Kiểm tra có bị block không
                if (await RelationshipHelper.IsBlockedAsync(_context, currentUserId, post.UserId))
                {
                    return null;
                }

                // Nếu là bài viết riêng tư
                if (post.Privacy == PrivacyLevel.Private) return null;

                // Nếu là bài viết chỉ bạn bè
                if (post.Privacy == PrivacyLevel.Friends)
                {
                    if (!await RelationshipHelper.IsFriendAsync(_context, currentUserId, post.UserId))
                    {
                        return null;
                    }
                }
            }
            else if (string.IsNullOrEmpty(currentUserId) && post.Privacy != PrivacyLevel.Public)
            {
                return null;
            }

            return await MapToDetailVModelAsync(post, currentUserId);
        }

        public async Task<PaginationModel<PostDetailVModel>> GetFriendsFeedAsync(PostFilterVModel filter)
        {
            var currentUserId = GlobalUserId;
            if (string.IsNullOrEmpty(currentUserId))
            {
                return new PaginationModel<PostDetailVModel> { TotalRecords = 0, Records = new List<PostDetailVModel>() };
            }

            // 1. Lấy danh sách bạn bè thân thiết (IsFriend == true)
            var friendIds = await RelationshipHelper.GetFriendIdsAsync(_context, currentUserId);

            // Đưa chính mình vào danh sách để thấy bài của bản thân trên feed
            var feedAuthorIds = new HashSet<string>(friendIds) { currentUserId };

            // 2. Lấy danh sách ID những người bị chặn (để loại trừ 2 chiều)
            var blockedUserIds = await RelationshipHelper.GetBlockedUserIdsAsync(_context, currentUserId);

            // 3. Query bài viết theo BuildQueryable kết hợp bạn bè và phân quyền
            var query = _context.Posts
                .Where(BuildQueryable(filter))
                .Where(p => !blockedUserIds.Contains(p.UserId) && feedAuthorIds.Contains(p.UserId))
                .Where(p => p.UserId == currentUserId || p.Privacy == PrivacyLevel.Friends || p.Privacy == PrivacyLevel.Public)
                .Include(p => p.User)
                .Include(p => p.PostMedias.Where(pm => pm.IsActive == true).OrderBy(pm => pm.DisplayOrder))
                    .ThenInclude(pm => pm.File)
                .Include(p => p.PostHashtags)
                    .ThenInclude(ph => ph.Hashtag);

            var totalRecords = await query.CountAsync();

            var posts = await query
                .OrderByDescending(p => p.IsPinned)
                .ThenByDescending(p => p.CreatedDate)
                .Skip((filter.PageNumber - 1) * filter.PageSize)
                .Take(filter.PageSize)
                .ToListAsync();

            var records = new List<PostDetailVModel>();
            foreach (var post in posts)
            {
                records.Add(await MapToDetailVModelAsync(post, currentUserId));
            }

            return new PaginationModel<PostDetailVModel>
            {
                TotalRecords = totalRecords,
                Records = records
            };
        }

        public async Task<PaginationModel<PostDetailVModel>> GetUserPostsAsync(string targetUserId, PostFilterVModel filter)
        {
            var currentUserId = GlobalUserId;

            // Kiểm tra bị chặn
            if (!string.IsNullOrEmpty(currentUserId) && currentUserId != targetUserId)
            {
                if (await RelationshipHelper.IsBlockedAsync(_context, currentUserId, targetUserId))
                {
                    return new PaginationModel<PostDetailVModel> { TotalRecords = 0, Records = new List<PostDetailVModel>() };
                }
            }

            bool isSelf = currentUserId == targetUserId;
            bool isFriend = false;

            if (!string.IsNullOrEmpty(currentUserId) && !isSelf)
            {
                isFriend = await RelationshipHelper.IsFriendAsync(_context, currentUserId, targetUserId);
            }

            IQueryable<Post> query = _context.Posts
                .Where(BuildQueryable(filter))
                .Where(p => p.UserId == targetUserId);

            // Lọc quyền riêng tư
            if (!isSelf)
            {
                if (isFriend)
                {
                    query = query.Where(p => p.Privacy == PrivacyLevel.Friends || p.Privacy == PrivacyLevel.Public);
                }
                else
                {
                    query = query.Where(p => p.Privacy == PrivacyLevel.Public);
                }
            }

            var totalRecords = await query.CountAsync();

            var posts = await query
                .Include(p => p.User)
                .Include(p => p.PostMedias.Where(pm => pm.IsActive == true).OrderBy(pm => pm.DisplayOrder))
                    .ThenInclude(pm => pm.File)
                .Include(p => p.PostHashtags)
                    .ThenInclude(ph => ph.Hashtag)
                .OrderByDescending(p => p.IsPinned)
                .ThenByDescending(p => p.CreatedDate)
                .Skip((filter.PageNumber - 1) * filter.PageSize)
                .Take(filter.PageSize)
                .ToListAsync();

            var records = new List<PostDetailVModel>();
            foreach (var post in posts)
            {
                records.Add(await MapToDetailVModelAsync(post, currentUserId));
            }

            return new PaginationModel<PostDetailVModel>
            {
                TotalRecords = totalRecords,
                Records = records
            };
        }

        public async Task<PaginationModel<PostDetailVModel>> GetPostsByHashtagAsync(string tag, PostFilterVModel filter)
        {
            var currentUserId = GlobalUserId;
            var normalizedTag = tag.TrimStart('#').ToLower();

            var query = _context.PostHashtags
                .Include(ph => ph.Post).ThenInclude(p => p.User)
                .Include(ph => ph.Post).ThenInclude(p => p.PostMedias.Where(pm => pm.IsActive == true).OrderBy(pm => pm.DisplayOrder)).ThenInclude(pm => pm.File)
                .Include(ph => ph.Post).ThenInclude(p => p.PostHashtags).ThenInclude(ph2 => ph2.Hashtag)
                .Where(ph => ph.Hashtag.Tag == normalizedTag && ph.Post.IsActive == true)
                .Select(ph => ph.Post);

            // Chỉ lấy bài viết Public (trừ khi là bài của chính mình)
            query = query.Where(p => p.Privacy == PrivacyLevel.Public || p.UserId == currentUserId);

            var totalRecords = await query.CountAsync();

            var posts = await query
                .OrderByDescending(p => p.CreatedDate)
                .Skip((filter.PageNumber - 1) * filter.PageSize)
                .Take(filter.PageSize)
                .ToListAsync();

            var records = new List<PostDetailVModel>();
            foreach (var post in posts)
            {
                records.Add(await MapToDetailVModelAsync(post, currentUserId));
            }

            return new PaginationModel<PostDetailVModel>
            {
                TotalRecords = totalRecords,
                Records = records
            };
        }

        #region Private Helpers

        private async Task ProcessHashtagsAsync(long postId, string? content)
        {
            if (string.IsNullOrWhiteSpace(content)) return;

            var matches = Regex.Matches(content, @"#(\w+)");
            var tags = matches.Select(m => m.Groups[1].Value.ToLower()).Distinct().ToList();

            foreach (var tag in tags)
            {
                var hashtag = await _context.Hashtags.FirstOrDefaultAsync(h => h.Tag == tag);
                if (hashtag == null)
                {
                    hashtag = new Hashtag
                    {
                        Tag = tag,
                        UsageCount = 1,
                        CreatedDate = DateTime.UtcNow,
                        IsActive = true
                    };
                    _context.Hashtags.Add(hashtag);
                    await _context.SaveChangesAsync();
                }
                else
                {
                    hashtag.UsageCount++;
                }

                _context.PostHashtags.Add(new PostHashtag
                {
                    PostId = postId,
                    HashtagId = hashtag.Id
                });
            }
        }

        private async Task<PostDetailVModel> MapToDetailVModelAsync(Post post, string? currentUserId)
        {
            var author = post.User;
            ReactionType? userReaction = null;

            if (!string.IsNullOrEmpty(currentUserId))
            {
                var reaction = await _context.Reactions
                    .FirstOrDefaultAsync(r => r.TargetType == ReactionTargetType.Post
                                           && r.TargetId == post.Id
                                           && r.UserId == currentUserId
                                           && r.IsActive == true);
                if (reaction != null)
                {
                    userReaction = reaction.Type;
                }
            }

            var topReactions = await _context.Reactions
                .Where(r => r.TargetType == ReactionTargetType.Post
                         && r.TargetId == post.Id
                         && r.IsActive == true)
                .GroupBy(r => r.Type)
                .OrderByDescending(g => g.Count())
                .Select(g => g.Key)
                .Take(3)
                .ToListAsync();

            return PostMappings.EntityToDetailVModel(post, currentUserId, userReaction, topReactions);
        }

        private static Expression<Func<Post, bool>> BuildQueryable(PostFilterVModel fParams)
        {
            return p =>
                (fParams.IsActive == null || p.IsActive == fParams.IsActive) &&
                (!fParams.GroupId.HasValue ? p.GroupId == null : p.GroupId == fParams.GroupId.Value) &&
                (!fParams.IsExpenseOnly.HasValue || (fParams.IsExpenseOnly.Value ? p.IsExpense : true)) &&
                (!fParams.ExpenseCategoryId.HasValue || p.ExpenseCategoryId == fParams.ExpenseCategoryId.Value) &&
                (!fParams.MinAmount.HasValue || p.Amount >= fParams.MinAmount.Value) &&
                (!fParams.MaxAmount.HasValue || p.Amount <= fParams.MaxAmount.Value) &&
                (string.IsNullOrEmpty(fParams.SearchString) || (p.Content != null && p.Content.Contains(fParams.SearchString)) || (p.FoodName != null && p.FoodName.Contains(fParams.SearchString))) &&
                (!fParams.FromDate.HasValue || p.CreatedDate >= fParams.FromDate.Value) &&
                (!fParams.ToDate.HasValue || p.CreatedDate <= fParams.ToDate.Value);
        }

        #endregion
    }
}
