using SL.Infrastructures.EntityFramework.Entities;

namespace SL.Domain.VModels.User
{
    public class RelationshipActionResponse
    {
        public bool IsSuccess { get; set; }
        public string? Message { get; set; }
        public RelationshipType? Type { get; set; }
        public RelationshipStatus? Status { get; set; }
    }

    public class RelationshipFilterVModel : SL.Domain.Common.Models.BaseFilterParams
    {
    }

    public class FriendRequestItemVModel
    {
        public long RequestId { get; set; }
        public string SenderId { get; set; } = null!;
        public string? SenderUserName { get; set; }
        public string? SenderFullName { get; set; }
        public string? SenderAvatarUrl { get; set; }
        public DateTime? CreatedDate { get; set; }
    }
}

