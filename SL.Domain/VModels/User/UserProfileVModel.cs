using SL.Domain.Common.Models;
using SL.Domain.Common.Ultilities;
using System.Text.Json.Serialization;

namespace SL.Domain.VModels.User
{
    public class UserProfileVModel
    {
        public string Id { get; set; } = null!;
        public string? UserName { get; set; }
        public string? Email { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? FullName { get; set; }
        public string? AvatarUrl { get; set; }
        public string? CoverImageUrl { get; set; }
        public bool? Sex { get; set; }

        [JsonConverter(typeof(DateOnlyJsonConverter))]
        public DateOnly? Birthday { get; set; }
        public string? Address { get; set; }
        public string? Bio { get; set; }
        public bool IsPrivate { get; set; }
        public bool IsVerified { get; set; }
        public int FollowersCount { get; set; }
        public int FollowingCount { get; set; }
        public int FriendsCount { get; set; }
        public int PostsCount { get; set; }
        public DateTime? CreatedDate { get; set; }

        // Trạng thái quan hệ với người dùng đang đăng nhập
        public bool IsFollowing { get; set; }
        public bool IsFollowedBy { get; set; }
        public bool IsFriend { get; set; }
        public bool HasSentFriendRequest { get; set; }
        public bool HasReceivedFriendRequest { get; set; }
        public bool IsBlocked { get; set; }
    }

    public class UpdateProfileRequest
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Bio { get; set; }
        public string? Address { get; set; }
        public bool? Sex { get; set; }

        [JsonConverter(typeof(DateOnlyJsonConverter))]
        public DateOnly? Birthday { get; set; }
        public bool? IsPrivate { get; set; }
    }

    public class UserSearchFilterVModel : BaseFilterParams
    {
        public string? Keyword { get => SearchString; set => SearchString = value; }
    }

    public class UserSummaryVModel
    {
        public string Id { get; set; } = null!;
        public string? UserName { get; set; }
        public string? FullName { get; set; }
        public string? AvatarUrl { get; set; }
        public string? Bio { get; set; }
        public bool IsVerified { get; set; }
        public bool IsFollowing { get; set; }
        public bool IsFriend { get; set; }
    }
}

