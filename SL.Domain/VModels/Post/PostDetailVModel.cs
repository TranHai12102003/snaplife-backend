using SL.Domain.VModels.User;
using SL.Infrastructures.EntityFramework.Entities;

namespace SL.Domain.VModels.Post
{
    public class PostDetailVModel
    {
        public long Id { get; set; }
        public string? Content { get; set; }
        public string? LocationName { get; set; }
        public double? Latitude { get; set; }
        public double? Longitude { get; set; }
        public PrivacyLevel Privacy { get; set; }
        public bool IsPinned { get; set; }
        public DateTime? CreatedDate { get; set; }

        // Thông tin tác giả
        public UserSummaryVModel Author { get; set; } = null!;

        // Danh sách ảnh/video
        public List<PostMediaVModel> Medias { get; set; } = new List<PostMediaVModel>();

        // Danh sách hashtag
        public List<string> Hashtags { get; set; } = new List<string>();

        // Quản lý chi tiêu thị giác (Visual Expense Tracking)
        public bool IsExpense { get; set; }
        public decimal? Amount { get; set; }
        public string? Currency { get; set; }
        public string? FoodName { get; set; }
        public long? ExpenseCategoryId { get; set; }
        public string? ExpenseCategoryName { get; set; }
        public bool ShowAmountToFriends { get; set; }

        // Hội nhóm (nếu có)
        public long? GroupId { get; set; }

        // Tương tác & Thống kê
        public int LikeCount { get; set; }
        public int CommentCount { get; set; }
        public int ShareCount { get; set; }

        // Trạng thái đối với người dùng đang đăng nhập
        public bool IsOwner { get; set; }
        public ReactionType? UserReaction { get; set; }

        // Top biểu cảm được thả nhiều nhất
        public List<ReactionType> TopReactions { get; set; } = new();
    }
}

