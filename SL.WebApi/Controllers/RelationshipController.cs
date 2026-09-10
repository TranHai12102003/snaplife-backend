using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SL.Domain.Common.Constants;
using SL.Domain.Common.Models;
using SL.Domain.IServices;
using SL.Domain.VModels.User;

namespace SL.WebApi.Controllers
{
    [Route(Strings.BaseRoute)]
    [ApiController]
    [Authorize]
    public class RelationshipController : ControllerBase
    {
        private readonly IRelationshipService _relationshipService;

        public RelationshipController(IRelationshipService relationshipService)
        {
            _relationshipService = relationshipService;
        }

        [HttpPost("Follow/{targetUserId}")]
        public async Task<ActionResult<RelationshipActionResponse>> Follow(string targetUserId)
        {
            var result = await _relationshipService.ToggleFollowAsync(targetUserId);
            if (!result.IsSuccess)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }

        [HttpPost("SendFriendRequest/{targetUserId}")]
        public async Task<ActionResult<RelationshipActionResponse>> SendFriendRequest(string targetUserId)
        {
            var result = await _relationshipService.SendFriendRequestAsync(targetUserId);
            if (!result.IsSuccess)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }

        [HttpPut("AcceptFriendRequest/{requestId}")]
        public async Task<ActionResult<RelationshipActionResponse>> AcceptFriendRequest(long requestId)
        {
            var result = await _relationshipService.AcceptFriendRequestAsync(requestId);
            if (!result.IsSuccess)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }

        [HttpDelete("DeclineFriendRequest/{requestId}")]
        public async Task<ActionResult<RelationshipActionResponse>> DeclineFriendRequest(long requestId)
        {
            var result = await _relationshipService.DeclineFriendRequestAsync(requestId);
            if (!result.IsSuccess)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }

        [HttpDelete("Unfriend/{targetUserId}")]
        public async Task<ActionResult<RelationshipActionResponse>> Unfriend(string targetUserId)
        {
            var result = await _relationshipService.UnfriendAsync(targetUserId);
            if (!result.IsSuccess)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }

        [HttpPost("Block/{targetUserId}")]
        public async Task<ActionResult<RelationshipActionResponse>> Block(string targetUserId)
        {
            var result = await _relationshipService.ToggleBlockAsync(targetUserId);
            if (!result.IsSuccess)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }

        [HttpGet("Followers/{userId}")]
        [AllowAnonymous]
        public async Task<ActionResult<PaginationModel<UserSummaryVModel>>> Followers(string userId, [FromQuery] RelationshipFilterVModel filter)
        {
            var result = await _relationshipService.GetFollowersAsync(userId, filter);
            return Ok(result);
        }

        [HttpGet("Following/{userId}")]
        [AllowAnonymous]
        public async Task<ActionResult<PaginationModel<UserSummaryVModel>>> Following(string userId, [FromQuery] RelationshipFilterVModel filter)
        {
            var result = await _relationshipService.GetFollowingAsync(userId, filter);
            return Ok(result);
        }

        [HttpGet("Friends/{userId}")]
        [AllowAnonymous]
        public async Task<ActionResult<PaginationModel<UserSummaryVModel>>> Friends(string userId, [FromQuery] RelationshipFilterVModel filter)
        {
            var result = await _relationshipService.GetFriendsAsync(userId, filter);
            return Ok(result);
        }

        [HttpGet("PendingFriendRequests")]
        public async Task<ActionResult<List<FriendRequestItemVModel>>> PendingFriendRequests()
        {
            var result = await _relationshipService.GetPendingFriendRequestsAsync();
            return Ok(result);
        }
    }
}

