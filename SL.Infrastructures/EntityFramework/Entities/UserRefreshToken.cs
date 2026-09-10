using SL.Infrastructures.EntityFramework.Entities.SysEntities;

namespace SL.Infrastructures.EntityFramework.Entities
{
    public class UserRefreshToken : BaseEntity
    {
        public string UserId { get; set; } = null!;
        public string Token { get; set; } = null!;
        public string? JwtId { get; set; }
        public bool IsUsed { get; set; } = false;
        public bool IsRevoked { get; set; } = false;
        public DateTime ExpiryDate { get; set; }

        public virtual AspNetUsers User { get; set; } = null!;
    }
}

