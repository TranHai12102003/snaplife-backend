using Microsoft.EntityFrameworkCore;
using SL.Domain.Common.Constants;
using SL.Infrastructures.EntityFramework;
using SL.Infrastructures.EntityFramework.Entities;

namespace SL.Services.Helpers
{
    public static class RelationshipHelper
    {
        public static async Task<List<string>> GetFriendIdsAsync(SnapLifeContext context, string userId)
        {
            if (string.IsNullOrEmpty(userId)) return new List<string>();

            return await context.UserRelationships
                .Where(r => (r.SourceUserId == userId || r.TargetUserId == userId)
                         && r.Type == RelationshipType.Friend
                         && r.Status == RelationshipStatus.Accepted
                         && r.IsActive == true)
                .Select(r => r.SourceUserId == userId ? r.TargetUserId : r.SourceUserId)
                .Distinct()
                .ToListAsync();
        }

        public static async Task<List<string>> GetBlockedUserIdsAsync(SnapLifeContext context, string userId)
        {
            if (string.IsNullOrEmpty(userId)) return new List<string>();

            return await context.UserRelationships
                .Where(r => (r.SourceUserId == userId || r.TargetUserId == userId)
                         && r.Type == RelationshipType.Block
                         && r.IsActive == true)
                .Select(r => r.SourceUserId == userId ? r.TargetUserId : r.SourceUserId)
                .Distinct()
                .ToListAsync();
        }

        public static async Task<bool> IsFriendAsync(SnapLifeContext context, string userId1, string userId2)
        {
            if (string.IsNullOrEmpty(userId1) || string.IsNullOrEmpty(userId2) || userId1 == userId2)
                return false;

            return await context.UserRelationships
                .AnyAsync(r => ((r.SourceUserId == userId1 && r.TargetUserId == userId2)
                             || (r.SourceUserId == userId2 && r.TargetUserId == userId1))
                            && r.Type == RelationshipType.Friend
                            && r.Status == RelationshipStatus.Accepted
                            && r.IsActive == true);
        }

        public static async Task<bool> IsBlockedAsync(SnapLifeContext context, string userId1, string userId2)
        {
            if (string.IsNullOrEmpty(userId1) || string.IsNullOrEmpty(userId2) || userId1 == userId2)
                return false;

            return await context.UserRelationships
                .AnyAsync(r => ((r.SourceUserId == userId1 && r.TargetUserId == userId2)
                             || (r.SourceUserId == userId2 && r.TargetUserId == userId1))
                            && r.Type == RelationshipType.Block
                            && r.IsActive == true);
        }
    }
}
