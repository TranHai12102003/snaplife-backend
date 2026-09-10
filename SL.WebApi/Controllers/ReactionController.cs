using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SL.Domain.Common.Constants;
using SL.Domain.IServices;
using SL.Domain.VModels.Reaction;

namespace SL.WebApi.Controllers
{
    [Route(Strings.BaseRoute)]
    [ApiController]
    public class ReactionController : ControllerBase
    {
        private readonly IReactionService _reactionService;

        public ReactionController(IReactionService reactionService)
        {
            _reactionService = reactionService;
        }

        [HttpPost("React/{postId}")]
        [Authorize]
        public async Task<ActionResult<ReactionActionResponse>> React(long postId, [FromBody] ReactionRequest request)
        {
            var result = await _reactionService.ReactToPostAsync(postId, request);
            if (!result.IsSuccess)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }

        [HttpGet("Post/{postId}")]
        [AllowAnonymous]
        public async Task<ActionResult<List<ReactionItemVModel>>> GetPostReactions(long postId)
        {
            var result = await _reactionService.GetPostReactionsAsync(postId);
            return Ok(result);
        }
    }
}

