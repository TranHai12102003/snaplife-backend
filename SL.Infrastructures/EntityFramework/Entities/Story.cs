using SL.Infrastructures.EntityFramework.Entities.SysEntities;

namespace SL.Infrastructures.EntityFramework.Entities
{
    public class Story : BaseEntity
    {
        public string UserId { get; set; } = null!;
        public long FileId { get; set; }
        public string? Caption { get; set; }
        public DateTime ExpiresAt { get; set; } = DateTime.UtcNow.AddHours(24);
        public int ViewCount { get; set; } = 0;
        public bool IsHighlight { get; set; } = false;

        public virtual AspNetUsers User { get; set; } = null!;
        public virtual SysFile File { get; set; } = null!;
        public virtual ICollection<StoryView> StoryViews { get; set; } = new List<StoryView>();
    }
}

