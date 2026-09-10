using SL.Domain.Common.Models;
using SL.Domain.VModels.Post;

namespace SL.Domain.IServices
{
    public interface IPostService
    {
        Task<ResponseResult> CreatePostAsync(PostCreateRequest request);
        Task<ResponseResult> UpdatePostAsync(long postId, PostUpdateRequest request);
        Task<ResponseResult> DeletePostAsync(long postId);
        Task<ResponseResult> TogglePinPostAsync(long postId);
        Task<PostDetailVModel?> GetPostByIdAsync(long postId);
        Task<PaginationModel<PostDetailVModel>> GetFriendsFeedAsync(PostFilterVModel filter);
        Task<PaginationModel<PostDetailVModel>> GetUserPostsAsync(string targetUserId, PostFilterVModel filter);
        Task<PaginationModel<PostDetailVModel>> GetPostsByHashtagAsync(string tag, PostFilterVModel filter);
    }
}

