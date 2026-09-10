using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using SL.Domain.Common.Constants;
using SL.Domain.Common.Models;
using SL.Domain.IServices.ISysServices;
using SL.Domain.VModels.SysVModels;
using SL.Infrastructures.EntityFramework;
using SL.Infrastructures.EntityFramework.Entities.SysEntities;
using SL.Services.Helpers;
using SL.Services.Mappings;

namespace SL.Services.Services.SysServices
{
    public class AuthService(
        UserManager<AspNetUsers> userManager,
        RoleManager<AspNetRoles> roleManager,
        IConfiguration configuration,
        IHttpContextAccessor contextAccessor,
        SnapLifeContext context) : Globals(contextAccessor), IAuthService
    {
        private readonly UserManager<AspNetUsers> _userManager = userManager;
        private readonly RoleManager<AspNetRoles> _roleManager = roleManager;
        private readonly IConfiguration _configuration = configuration;
        private readonly SnapLifeContext _context = context;

        public async Task<LoginResponse> Login(LoginVModel model)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null)
            {
                return new LoginResponse
                {
                    IsSuccess = false,
                    Message = Strings.Messages.NotFoundEmail
                };
            }

            if (user.IsActive != true)
            {
                return new LoginResponse
                {
                    IsSuccess = false,
                    Message = Strings.Messages.InActiveAccount
                };
            }

            var isPasswordValid = await _userManager.CheckPasswordAsync(user, model.Password);
            if (!isPasswordValid)
            {
                return new LoginResponse
                {
                    IsSuccess = false,
                    Message = Strings.Messages.InValidPasswword
                };
            }

            var roles = await _userManager.GetRolesAsync(user);
            var token = GenerateToken.GenerateTokenJWT(_configuration, user.Id, user.Email, user.UserName, roles);

            return new LoginResponse
            {
                IsSuccess = true,
                Message = "Login successfully.",
                Token = token,
                User = AuthMappings.EntityToVModel(user, roles)
            };
        }

        public async Task<RegisterResponse> Register(RegisterVModel model)
        {
            var existingUser = await _userManager.FindByEmailAsync(model.Email);
            if (existingUser != null)
            {
                return new RegisterResponse
                {
                    IsSuccess = false,
                    Message = Strings.Messages.EmailAlreadyExists
                };
            }

            var userName = string.IsNullOrWhiteSpace(model.UserName)
                ? model.Email.Split('@')[0]
                : model.UserName.Trim();

            // Kiểm tra nếu UserName đã tồn tại thì thêm hậu tố ngẫu nhiên
            var userByUsername = await _userManager.FindByNameAsync(userName);
            if (userByUsername != null)
            {
                userName = $"{userName}_{Guid.NewGuid().ToString("N")[..6]}";
            }

            var user = new AspNetUsers
            {
                UserName = userName,
                Email = model.Email,
                FirstName = model.FirstName,
                LastName = model.LastName,
                IsActive = true,
                CreatedDate = DateTime.UtcNow
            };

            var createResult = await _userManager.CreateAsync(user, model.Password);
            if (!createResult.Succeeded)
            {
                var errors = string.Join(", ", createResult.Errors.Select(e => e.Description));
                return new RegisterResponse
                {
                    IsSuccess = false,
                    Message = string.Format(Strings.Messages.GeneralError, errors)
                };
            }

            // Đảm bảo role Member tồn tại và gán cho user mới
            if (!await _roleManager.RoleExistsAsync(Strings.StaticRoles.Mem))
            {
                await _roleManager.CreateAsync(new AspNetRoles
                {
                    Name = Strings.StaticRoles.Mem,
                    NormalizedName = Strings.StaticRoles.Mem.ToUpperInvariant(),
                    Description = "Regular Member"
                });
            }

            await _userManager.AddToRoleAsync(user, Strings.StaticRoles.Mem);
            var roles = await _userManager.GetRolesAsync(user);

            return new RegisterResponse
            {
                IsSuccess = true,
                Message = Strings.Messages.RegistrationSuccessful,
                User = AuthMappings.EntityToVModel(user, roles)
            };
        }

        public async Task<ResponseResult> Me()
        {
            try
            {
                if (string.IsNullOrEmpty(GlobalUserId))
                {
                    return new ErrorResponseResult(Strings.Messages.UserNotAuthenticated);
                }

                var user = await _userManager.FindByIdAsync(GlobalUserId);
                if (user == null)
                {
                    return new ErrorResponseResult(Strings.Messages.NotFound);
                }

                var roles = await _userManager.GetRolesAsync(user);
                var userModel = AuthMappings.EntityToVModel(user, roles);

                return new SuccessResponseResult(userModel, "Get profile successfully.");
            }
            catch (Exception ex)
            {
                return new ErrorResponseResult(ex.Message);
            }
        }

        public async Task<ResponseResult> UpdateAvatar(long fileId)
        {
            if (string.IsNullOrEmpty(GlobalUserId))
            {
                return new ErrorResponseResult(Strings.Messages.UserNotAuthenticated);
            }

            var user = await _userManager.FindByIdAsync(GlobalUserId);
            if (user == null)
            {
                return new ErrorResponseResult(Strings.Messages.NotFound);
            }

            var file = await _context.SysFiles.FindAsync(fileId);
            if (file == null)
            {
                return new ErrorResponseResult("File not found in system.");
            }

            user.AvatarUrl = file.FilePath;
            user.UpdatedDate = DateTime.UtcNow;
            user.UpdatedBy = GlobalUserId;

            var updateResult = await _userManager.UpdateAsync(user);
            if (!updateResult.Succeeded)
            {
                return new ErrorResponseResult("Failed to update avatar.");
            }

            var roles = await _userManager.GetRolesAsync(user);
            return new SuccessResponseResult(AuthMappings.EntityToVModel(user, roles), "Avatar updated successfully.");
        }

        public async Task<ResponseResult> UpdateCoverImage(long fileId)
        {
            if (string.IsNullOrEmpty(GlobalUserId))
            {
                return new ErrorResponseResult(Strings.Messages.UserNotAuthenticated);
            }

            var user = await _userManager.FindByIdAsync(GlobalUserId);
            if (user == null)
            {
                return new ErrorResponseResult(Strings.Messages.NotFound);
            }

            var file = await _context.SysFiles.FindAsync(fileId);
            if (file == null)
            {
                return new ErrorResponseResult("File not found in system.");
            }

            user.CoverImageUrl = file.FilePath;
            user.UpdatedDate = DateTime.UtcNow;
            user.UpdatedBy = GlobalUserId;

            var updateResult = await _userManager.UpdateAsync(user);
            if (!updateResult.Succeeded)
            {
                return new ErrorResponseResult("Failed to update cover image.");
            }

            var roles = await _userManager.GetRolesAsync(user);
            return new SuccessResponseResult(AuthMappings.EntityToVModel(user, roles), "Cover image updated successfully.");
        }
    }
}

