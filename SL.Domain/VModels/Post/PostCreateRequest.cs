using SL.Infrastructures.EntityFramework.Entities;
using System.ComponentModel.DataAnnotations;

namespace SL.Domain.VModels.Post
{
    public class PostCreateRequest
    {
        [MaxLength(1000)]
        public string? Content { get; set; }

        public string? LocationName { get; set; }
        public double? Latitude { get; set; }
        public double? Longitude { get; set; }

        // Danh sách ID của ảnh/video đã upload từ Module 2 (MediaFiles)
        public List<long> MediaFileIds { get; set; } = new List<long>();

        // Mặc định là Friends theo đúng tinh thần Locket
        public PrivacyLevel Privacy { get; set; } = PrivacyLevel.Friends;

        // Quản lý chi tiêu thị giác (Visual Expense Tracking)
        public bool IsExpense { get; set; } = false;
        public decimal? Amount { get; set; }
        public string Currency { get; set; } = "VND";
        public string? FoodName { get; set; }
        public long? ExpenseCategoryId { get; set; }
        public bool ShowAmountToFriends { get; set; } = true;

        // Hội nhóm (nếu đăng trong Squad)
        public long? GroupId { get; set; }
    }
}

