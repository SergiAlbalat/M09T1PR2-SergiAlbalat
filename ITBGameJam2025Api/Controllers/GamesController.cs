﻿using ITBGameJam2025Api.Data;
using ITBGameJam2025Api.DTOs;
using ITBGameJam2025Api.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ITBGameJam2025Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GamesController : ControllerBase
    {
        private readonly AppDbContext _context;
        public GamesController(AppDbContext context)
        {
            _context = context;
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Game>>> GetGames()
        {
            var games = await _context.Games.ToListAsync();
            if (games.Count == 0)
            {
                return NotFound("There is no games");
            }
            return Ok(games);
        }
        [HttpPost]
        public async Task<ActionResult<Game>> PostGame(GameDTO gameDTO)
        {
            Game game = new Game
            {
                Title = gameDTO.Title,
                Description = gameDTO.Description,
                Developer = gameDTO.Developer,
                Image = gameDTO.Image
            };
            _context.Games.Add(game);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetGames), game);
        }
        [HttpPut]
        public async Task<ActionResult<Game>> PutGame(Game game)
        {
            Game gameToUpdate = await _context.Games.FirstOrDefaultAsync(x => x.Id == game.Id);
            gameToUpdate.Title = game.Title;
            gameToUpdate.Description = game.Description;
            gameToUpdate.Developer = game.Developer;
            gameToUpdate.Image = game.Image;
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetGames), game);
        }
        [HttpDelete]
        public async Task<ActionResult<Game>> DeleteGame(Game game)
        {
            _context.Games.Remove(game);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetGames), game);
        }
    }
}
