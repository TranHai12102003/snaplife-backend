using SL.Infrastructures.EntityFramework.Entities.SysEntities;

namespace SL.Infrastructures.EntityFramework.Entities
{
    public class PostMedia : BaseEntity
    {
        public long PostId { get; set; }
        public long FileId { get; set; }
        public int DisplayOrder { get; set; } = 0;
        public string? MediaType { get; set; } // Image, Video

        public virtual Post Post { get; set; } = null!;
        public virtual SysFile File { get; set; } = null!;
    }
}

