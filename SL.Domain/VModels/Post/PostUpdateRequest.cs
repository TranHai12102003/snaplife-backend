using SL.Infrastructures.EntityFramework.Entities;
using System.ComponentModel.DataAnnotations;

namespace SL.Domain.VModels.Post
{
    public class PostUpdateRequest
    {
        [MaxLength(1000)]
        public string? Content { get; set; }

        public string? LocationName { get; set; }
        public double? Latitude { get; set; }
        public double? Longitude { get; set; }

        public PrivacyLevel Privacy { get; set; } = PrivacyLevel.Friends;
        public bool IsPinned { get; set; } = false;

        // Cập nhật thông tin chi tiêu
        public bool IsExpense { get; set; } = false;
        public decimal? Amount { get; set; }
        public string Currency { get; set; } = "VND";
        public string? FoodName { get; set; }
        public long? ExpenseCategoryId { get; set; }
        public bool ShowAmountToFriends { get; set; } = true;
    }
}

