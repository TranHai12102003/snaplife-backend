using SL.Domain.VModels.Reaction;
using SL.Domain.VModels.User;
using SL.Infrastructures.EntityFramework.Entities;

namespace SL.Services.Mappings
{
    public static class ReactionMappings
    {
        public static ReactionItemVModel EntityToItemVModel(Reaction r)
        {
            return new ReactionItemVModel
            {
                Id = r.Id,
                Type = r.Type,
                CreatedDate = r.CreatedDate,
                User = r.User != null ? new UserSummaryVModel
                {
                    Id = r.User.Id,
                    UserName = r.User.UserName,
                    FullName = r.User.FullName,
                    AvatarUrl = r.User.AvatarUrl,
                    Bio = r.User.Bio,
                    IsVerified = r.User.IsVerified
                } : new UserSummaryVModel()
            };
        }
    }
}
