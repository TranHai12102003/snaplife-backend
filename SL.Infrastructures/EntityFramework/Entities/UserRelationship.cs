using SL.Infrastructures.EntityFramework.Entities.SysEntities;

namespace SL.Infrastructures.EntityFramework.Entities
{
    public class UserRelationship : BaseEntity
    {
        public string SourceUserId { get; set; } = null!;
        public string TargetUserId { get; set; } = null!;
        public RelationshipType Type { get; set; }
        public RelationshipStatus Status { get; set; } = RelationshipStatus.Accepted;

        public virtual AspNetUsers SourceUser { get; set; } = null!;
        public virtual AspNetUsers TargetUser { get; set; } = null!;
    }
}

