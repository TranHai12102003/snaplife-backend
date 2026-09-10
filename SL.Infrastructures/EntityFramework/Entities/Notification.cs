using SL.Infrastructures.EntityFramework.Entities.SysEntities;

namespace SL.Infrastructures.EntityFramework.Entities
{
    public class Notification : BaseEntity
    {
        public string ReceiverId { get; set; } = null!;
        public string? ActorId { get; set; }
        public NotificationType Type { get; set; }
        public long? EntityId { get; set; } // PostId, CommentId...
        public string Message { get; set; } = null!;
        public bool IsRead { get; set; } = false;
        public DateTime? ReadAt { get; set; }

        public virtual AspNetUsers Receiver { get; set; } = null!;
        public virtual AspNetUsers? Actor { get; set; }
    }
}

