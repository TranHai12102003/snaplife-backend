using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace SL.Infrastructures.EntityFramework.Entities.SysEntities
{
    public class AspNetUsers : IdentityUser
    {
        public string? FirstName { get; set; }

        public string? LastName { get; set; }

        public string? AvatarUrl { get; set; }

        public string? CoverImageUrl { get; set; }

        public bool? Sex { get; set; }

        public DateOnly? Birthday { get; set; }

        public string? Address { get; set; }

        public string? Bio { get; set; }

        public bool IsPrivate { get; set; } = false;

        public bool IsVerified { get; set; } = false;

        // Denormalized counts for high-performance profile view
        public int FollowersCount { get; set; } = 0;

        public int FollowingCount { get; set; } = 0;

        public int PostsCount { get; set; } = 0;

        public DateTime? CreatedDate { get; set; } = DateTime.UtcNow;

        public string? CreatedBy { get; set; }

        public DateTime? UpdatedDate { get; set; }

        public string? UpdatedBy { get; set; }

        public bool? IsActive { get; set; } = true;

        [NotMapped]
        public string FullName => $"{FirstName} {LastName}".Trim();
    }
}
