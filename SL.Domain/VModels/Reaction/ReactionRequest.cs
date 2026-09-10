using SL.Infrastructures.EntityFramework.Entities;

namespace SL.Domain.VModels.Reaction
{
    public class ReactionRequest
    {
        public ReactionType Type { get; set; } = ReactionType.Love;
    }
}

