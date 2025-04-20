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

        [Authorize]
        [HttpPost("Vote")]
        public async Task<ActionResult> PostVote(int gameId)
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

        [Authorize]
        [HttpPost("Unvote")]
        public async Task<ActionResult> PostUnvote(int gameId)
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

        [HttpGet("GetVotes/{gameId}")]
        public async Task<ActionResult<IEnumerable<GameVote>>> GetVotesByGameId(int gameId)
        {
            var gameVotes = await _context.GameVotes.Where(x => x.GameId == gameId).ToListAsync();
            return Ok(gameVotes.Count);
        }
    }
}
