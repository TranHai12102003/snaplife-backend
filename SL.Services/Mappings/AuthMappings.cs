using SL.Domain.VModels.SysVModels;
using SL.Infrastructures.EntityFramework.Entities.SysEntities;

namespace SL.Services.Mappings
{
    public static class AuthMappings
    {
        public static MeVModel EntityToVModel(AspNetUsers entity, IList<string>? roles = null)
        {
            return new MeVModel
            {
                Id = entity.Id,
                UserName = entity.UserName,
                Email = entity.Email,
                FirstName = entity.FirstName,
                LastName = entity.LastName,
                FullName = entity.FullName,
                AvatarUrl = entity.AvatarUrl,
                Sex = entity.Sex,
                Birthday = entity.Birthday,
                Address = entity.Address,
                Bio = entity.Bio,
                IsActive = entity.IsActive,
                CreatedDate = entity.CreatedDate,
                Roles = roles ?? new List<string>()
            };
        }
    }
}

