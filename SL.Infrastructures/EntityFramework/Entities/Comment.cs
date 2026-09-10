using SL.Infrastructures.EntityFramework.Entities.SysEntities;

namespace SL.Infrastructures.EntityFramework.Entities
{
    public class Comment : BaseEntity
    {
        public long PostId { get; set; }
        public string UserId { get; set; } = null!;
        public long? ParentCommentId { get; set; }
        public string Content { get; set; } = null!;
        public int LikeCount { get; set; } = 0;
        public bool IsEdited { get; set; } = false;

        public virtual Post Post { get; set; } = null!;
        public virtual AspNetUsers User { get; set; } = null!;
        public virtual Comment? ParentComment { get; set; }
        public virtual ICollection<Comment> Replies { get; set; } = new List<Comment>();
    }
}

