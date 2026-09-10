using SL.Infrastructures.EntityFramework.Entities.SysEntities;

namespace SL.Infrastructures.EntityFramework.Entities
{
    public class StoryView : BaseEntity
    {
        public long StoryId { get; set; }
        public string ViewerUserId { get; set; } = null!;
        public DateTime ViewedAt { get; set; } = DateTime.UtcNow;

        public virtual Story Story { get; set; } = null!;
        public virtual AspNetUsers ViewerUser { get; set; } = null!;
    }
}

