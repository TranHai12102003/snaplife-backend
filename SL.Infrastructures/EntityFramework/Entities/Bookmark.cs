using SL.Infrastructures.EntityFramework.Entities.SysEntities;

namespace SL.Infrastructures.EntityFramework.Entities
{
    public class Bookmark : BaseEntity
    {
        public string UserId { get; set; } = null!;
        public long PostId { get; set; }
        public string CollectionName { get; set; } = "All";

        public virtual AspNetUsers User { get; set; } = null!;
        public virtual Post Post { get; set; } = null!;
    }
}

