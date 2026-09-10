using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SL.Domain.Common.Constants;
using SL.Domain.Common.Models;
using SL.Domain.IServices;
using SL.Domain.VModels.User;

namespace SL.WebApi.Controllers
{
    [Route(Strings.ActionRoute)]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet("{userIdOrUserName}")]
        public async Task<ActionResult<UserProfileVModel>> GetProfile(string userIdOrUserName)
        {
            var profile = await _userService.GetProfileAsync(userIdOrUserName);
            if (profile == null)
            {
                return NotFound(new { message = "User not found." });
            }
            return Ok(profile);
        }

        [HttpPut]
        [Authorize]
        public async Task<ActionResult<ResponseResult>> UpdateProfile([FromBody] UpdateProfileRequest request)
        {
            var result = await _userService.UpdateProfileAsync(request);
            if (!result.IsSuccess)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }

        [HttpGet]
        public async Task<ActionResult<PaginationModel<UserSummaryVModel>>> Search([FromQuery] UserSearchFilterVModel filter)
        {
            var result = await _userService.SearchUsersAsync(filter);
            return Ok(result);
        }
    }
}

