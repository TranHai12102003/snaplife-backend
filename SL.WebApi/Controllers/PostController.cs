using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SL.Domain.Common.Constants;
using SL.Domain.Common.Models;
using SL.Domain.IServices;
using SL.Domain.VModels.Post;

namespace SL.WebApi.Controllers
{
    [Route(Strings.BaseRoute)]
    [ApiController]
    public class PostController : ControllerBase
    {
        private readonly IPostService _postService;

        public PostController(IPostService postService)
        {
            _postService = postService;
        }

        [HttpPost("Create")]
        [Authorize]
        public async Task<ActionResult<ResponseResult>> Create([FromBody] PostCreateRequest request)
        {
            var result = await _postService.CreatePostAsync(request);
            if (!result.IsSuccess)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }

        [HttpPut("Update/{id}")]
        [Authorize]
        public async Task<ActionResult<ResponseResult>> Update(long id, [FromBody] PostUpdateRequest request)
        {
            var result = await _postService.UpdatePostAsync(id, request);
            if (!result.IsSuccess)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }

        [HttpDelete("Delete/{id}")]
        [Authorize]
        public async Task<ActionResult<ResponseResult>> Delete(long id)
        {
            var result = await _postService.DeletePostAsync(id);
            if (!result.IsSuccess)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }

        [HttpPost("TogglePin/{id}")]
        [Authorize]
        public async Task<ActionResult<ResponseResult>> TogglePin(long id)
        {
            var result = await _postService.TogglePinPostAsync(id);
            if (!result.IsSuccess)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }

        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<ActionResult<PostDetailVModel>> GetById(long id)
        {
            var post = await _postService.GetPostByIdAsync(id);
            if (post == null)
            {
                return NotFound(new { message = "Snap not found or access restricted." });
            }
            return Ok(post);
        }

        [HttpGet("Feed")]
        [Authorize]
        public async Task<ActionResult<PaginationModel<PostDetailVModel>>> FriendsFeed([FromQuery] PostFilterVModel filter)
        {
            var result = await _postService.GetFriendsFeedAsync(filter);
            return Ok(result);
        }

        [HttpGet("User/{userId}")]
        [AllowAnonymous]
        public async Task<ActionResult<PaginationModel<PostDetailVModel>>> UserPosts(string userId, [FromQuery] PostFilterVModel filter)
        {
            var result = await _postService.GetUserPostsAsync(userId, filter);
            return Ok(result);
        }

        [HttpGet("Hashtag/{tag}")]
        [AllowAnonymous]
        public async Task<ActionResult<PaginationModel<PostDetailVModel>>> HashtagPosts(string tag, [FromQuery] PostFilterVModel filter)
        {
            var result = await _postService.GetPostsByHashtagAsync(tag, filter);
            return Ok(result);
        }
    }
}

