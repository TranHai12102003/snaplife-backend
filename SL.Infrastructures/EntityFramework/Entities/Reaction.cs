using SL.Infrastructures.EntityFramework.Entities.SysEntities;

namespace SL.Infrastructures.EntityFramework.Entities
{
    public class Reaction : BaseEntity
    {
        public string UserId { get; set; } = null!;
        public ReactionTargetType TargetType { get; set; }
        public long TargetId { get; set; }
        public ReactionType Type { get; set; } = ReactionType.Like;

        public virtual AspNetUsers User { get; set; } = null!;
    }
}

