using ITBGameJam2025Api.Data;
using ITBGameJam2025Api.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace ITBGameJam2025Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GameVotesController : ControllerBase
    {
        private readonly AppDbContext _context;
        public GameVotesController(AppDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Method for inserting a new vote
        /// </summary>
        /// <param name="gameId">The id of the game voted</param>
        /// <returns></returns>
        [Authorize]
        [HttpPost("Vote")]
        public async Task<ActionResult> PostVote([FromBody]int gameId)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var gameVote = new GameVote
            {
                GameId = gameId,
                UserId = userId
            };
            _context.GameVotes.Add(gameVote);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(PostVote), gameVote);
        }

        /// <summary>
        /// Method for removing a vote
        /// </summary>
        /// <param name="gameId">The id of the game unvoted</param>
        /// <returns></returns>
        [Authorize]
        [HttpPost("Unvote")]
        public async Task<ActionResult> PostUnvote([FromBody]int gameId)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var gameVote = await _context.GameVotes.FirstOrDefaultAsync(x => x.GameId == gameId && x.UserId == userId);
            if (gameVote == null)
            {
                return NotFound("Vote not found");
            }
            _context.GameVotes.Remove(gameVote);
            await _context.SaveChangesAsync();
            return Ok();
        }

        /// <summary>
        /// Method for getting all votes of the logged user
        /// </summary>
        /// <returns>All the votes of the user in a json array</returns>
        [Authorize]
        [HttpGet("GetVotes")]
        public async Task<ActionResult<IEnumerable<GameVote>>> GetVotes()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var gameVotes = await _context.GameVotes.Where(x => x.UserId == userId).ToListAsync();
            if (gameVotes.Count == 0)
            {
                return NotFound("No votes found");
            }
            return Ok(gameVotes);
        }

        /// <summary>
        /// Method for getting the number of votes of a game
        /// </summary>
        /// <param name="gameId">The id of the game</param>
        /// <returns>The number of votes that the game have</returns>
        [HttpGet("GetVotes/{gameId}")]
        public async Task<ActionResult<IEnumerable<GameVote>>> GetVotesByGameId(int gameId)
        {
            var gameVotes = await _context.GameVotes.Where(x => x.GameId == gameId).ToListAsync();
            return Ok(gameVotes.Count);
        }
    }
}
