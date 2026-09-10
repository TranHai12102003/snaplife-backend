using SL.Domain.Common.Constants;
using SL.Domain.VModels.Post;
using SL.Domain.VModels.User;
using SL.Infrastructures.EntityFramework.Entities;

namespace SL.Services.Mappings
{
    public static class PostMappings
    {
        public static Post CreateRequestToEntity(PostCreateRequest request, string userId)
        {
            return new Post
            {
                UserId = userId,
                Content = request.Content,
                LocationName = request.LocationName,
                Latitude = request.Latitude,
                Longitude = request.Longitude,
                Privacy = request.Privacy,
                IsPinned = false,
                IsExpense = request.IsExpense,
                Amount = request.IsExpense ? request.Amount : null,
                Currency = request.Currency ?? "VND",
                FoodName = request.IsExpense ? request.FoodName : null,
                ExpenseCategoryId = request.IsExpense ? request.ExpenseCategoryId : null,
                ShowAmountToFriends = request.ShowAmountToFriends,
                GroupId = request.GroupId,
                CreatedDate = DateTime.UtcNow,
                CreatedBy = userId,
                IsActive = true
            };
        }

        public static PostMediaVModel MediaToVModel(PostMedia pm)
        {
            return new PostMediaVModel
            {
                Id = pm.Id,
                FileId = pm.FileId,
                FileUrl = pm.File?.FilePath ?? string.Empty,
                ThumbnailUrl = null,
                MediaType = pm.MediaType,
                DisplayOrder = pm.DisplayOrder
            };
        }

        public static PostDetailVModel EntityToDetailVModel(
            Post post,
            string? currentUserId,
            ReactionType? userReaction = null)
        {
            var author = post.User;
            bool isSelf = !string.IsNullOrEmpty(currentUserId) && currentUserId == post.UserId;

            decimal? displayAmount = post.Amount;
            if (post.IsExpense && !post.ShowAmountToFriends && !isSelf)
            {
                displayAmount = null;
            }

            return new PostDetailVModel
            {
                Id = post.Id,
                Content = post.Content,
                LocationName = post.LocationName,
                Latitude = post.Latitude,
                Longitude = post.Longitude,
                Privacy = post.Privacy,
                IsPinned = post.IsPinned,
                CreatedDate = post.CreatedDate,
                Author = author != null ? new UserSummaryVModel
                {
                    Id = author.Id,
                    UserName = author.UserName,
                    FullName = author.FullName,
                    AvatarUrl = author.AvatarUrl,
                    Bio = author.Bio,
                    IsVerified = author.IsVerified
                } : new UserSummaryVModel(),
                Medias = post.PostMedias != null
                    ? post.PostMedias
                        .Where(pm => pm.IsActive == true)
                        .OrderBy(pm => pm.DisplayOrder)
                        .Select(MediaToVModel)
                        .ToList()
                    : new List<PostMediaVModel>(),
                Hashtags = post.PostHashtags != null
                    ? post.PostHashtags
                        .Select(ph => ph.Hashtag?.Tag ?? string.Empty)
                        .Where(t => !string.IsNullOrEmpty(t))
                        .ToList()
                    : new List<string>(),
                IsExpense = post.IsExpense,
                Amount = displayAmount,
                Currency = post.Currency,
                FoodName = post.FoodName,
                ExpenseCategoryId = post.ExpenseCategoryId,
                ShowAmountToFriends = post.ShowAmountToFriends,
                GroupId = post.GroupId,
                LikeCount = post.LikeCount,
                CommentCount = post.CommentCount,
                ShareCount = post.ShareCount,
                IsOwner = isSelf,
                UserReaction = userReaction
            };
        }
    }
}
