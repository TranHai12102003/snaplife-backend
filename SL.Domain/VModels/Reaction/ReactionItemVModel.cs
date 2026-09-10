using SL.Domain.VModels.User;
using SL.Infrastructures.EntityFramework.Entities;

namespace SL.Domain.VModels.Reaction
{
    public class ReactionItemVModel
    {
        public long Id { get; set; }
        public ReactionType Type { get; set; }
        public DateTime? CreatedDate { get; set; }
        public UserSummaryVModel User { get; set; } = null!;
    }
}

