using SL.Infrastructures.EntityFramework.Entities;

namespace SL.Domain.VModels.Reaction
{
    public class ReactionActionResponse
    {
        public bool IsSuccess { get; set; }
        public string? Message { get; set; }
        public bool IsReacted { get; set; }
        public ReactionType? CurrentReaction { get; set; }
        public int TotalReactions { get; set; }
    }
}

