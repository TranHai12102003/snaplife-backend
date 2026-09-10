using SL.Infrastructures.EntityFramework.Entities.SysEntities;

namespace SL.Infrastructures.EntityFramework.Entities
{
    public class Hashtag : BaseEntity
    {
        public string Tag { get; set; } = null!;
        public int UsageCount { get; set; } = 0;

        public virtual ICollection<PostHashtag> PostHashtags { get; set; } = new List<PostHashtag>();
    }
}

