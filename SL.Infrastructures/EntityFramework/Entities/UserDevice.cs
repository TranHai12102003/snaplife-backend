using SL.Infrastructures.EntityFramework.Entities.SysEntities;

namespace SL.Infrastructures.EntityFramework.Entities
{
    public class UserDevice : BaseEntity
    {
        public string UserId { get; set; } = null!;
        public string DeviceToken { get; set; } = null!;
        public string? DeviceType { get; set; } // iOS, Android, Web
        public string? DeviceName { get; set; }
        public DateTime LastActive { get; set; } = DateTime.UtcNow;

        public virtual AspNetUsers User { get; set; } = null!;
    }
}

