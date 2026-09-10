using SL.Domain.VModels.Reaction;

namespace SL.Domain.IServices
{
    public interface IReactionService
    {
        Task<ReactionActionResponse> ReactToPostAsync(long postId, ReactionRequest request);
        Task<List<ReactionItemVModel>> GetPostReactionsAsync(long postId);
    }
}

