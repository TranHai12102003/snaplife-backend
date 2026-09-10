using SL.Infrastructures.EntityFramework.Entities.SysEntities;

namespace SL.Infrastructures.EntityFramework.Entities
{
    public class Post : BaseEntity
    {
        public string UserId { get; set; } = null!;
        public string? Content { get; set; }
        public string? LocationName { get; set; }
        public double? Latitude { get; set; }
        public double? Longitude { get; set; }
        public PrivacyLevel Privacy { get; set; } = PrivacyLevel.Friends;
        public bool IsPinned { get; set; } = false;

        // Chi tiêu thị giác (Visual Expense Tracking)
        public bool IsExpense { get; set; } = false;
        public decimal? Amount { get; set; }
        public string Currency { get; set; } = "VND";
        public string? FoodName { get; set; }
        public long? ExpenseCategoryId { get; set; }
        public bool ShowAmountToFriends { get; set; } = true;

        // Hội nhóm (Squad / Group)
        public long? GroupId { get; set; }

        // Bộ đếm phi chuẩn hóa phục vụ query bảng tin tốc độ cao
        public int LikeCount { get; set; } = 0;
        public int CommentCount { get; set; } = 0;
        public int ShareCount { get; set; } = 0;

        public virtual AspNetUsers User { get; set; } = null!;
        public virtual ExpenseCategory? ExpenseCategory { get; set; }
        public virtual ICollection<PostMedia> PostMedias { get; set; } = new List<PostMedia>();
        public virtual ICollection<Comment> Comments { get; set; } = new List<Comment>();
        public virtual ICollection<PostHashtag> PostHashtags { get; set; } = new List<PostHashtag>();
    }
}

