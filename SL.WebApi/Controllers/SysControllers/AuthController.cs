using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SL.Domain.Common.Constants;
using SL.Domain.Common.Models;
using SL.Domain.IServices.ISysServices;
using SL.Domain.VModels.Media;
using SL.Domain.VModels.SysVModels;

namespace SL.WebApi.Controllers.SysControllers
{
    [Route(Strings.ActionRoute)]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost]
        public async Task<ActionResult<RegisterResponse>> Register([FromBody] RegisterVModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _authService.Register(model);
            if (!result.IsSuccess)
            {
                return BadRequest(result);
            }

            return Ok(result);
        }

        [HttpPost]
        public async Task<ActionResult<LoginResponse>> Login([FromBody] LoginVModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _authService.Login(model);
            if (!result.IsSuccess)
            {
                return BadRequest(result);
            }

            return Ok(result);
        }

        [HttpGet]
        [Authorize]
        public async Task<ActionResult<ResponseResult>> Me()
        {
            var result = await _authService.Me();
            if (!result.IsSuccess)
            {
                return BadRequest(result);
            }

            return Ok(result);
        }

        [HttpPut]
        [Authorize]
        public async Task<ActionResult<ResponseResult>> UpdateAvatar([FromBody] UpdateAvatarRequest request)
        {
            var result = await _authService.UpdateAvatar(request.FileId);
            if (!result.IsSuccess)
            {
                return BadRequest(result);
            }

            return Ok(result);
        }

        [HttpPut]
        [Authorize]
        public async Task<ActionResult<ResponseResult>> UpdateCover([FromBody] UpdateCoverRequest request)
        {
            var result = await _authService.UpdateCoverImage(request.FileId);
            if (!result.IsSuccess)
            {
                return BadRequest(result);
            }

            return Ok(result);
        }
    }
}

