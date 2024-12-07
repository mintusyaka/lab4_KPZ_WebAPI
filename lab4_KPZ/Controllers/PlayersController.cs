using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using lab4_KPZ.Data;
using lab4_KPZ.Models;
using AutoMapper;
using lab4_KPZ.ViewModels;

namespace lab4_KPZ.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PlayersController : ControllerBase
    {
        private readonly CharityGameContext _context;
		private readonly IMapper _mapper;

		public PlayersController(CharityGameContext context, IMapper mapper)
        {
            _context = context;
			_mapper = mapper;
        }

        // GET: api/Players
        [HttpGet]
        public async Task<ActionResult<IEnumerable<PlayerViewModel>>> GetPlayers()
        {
			var players = await _context.Players.ToListAsync();

			if (players == null)
				return NotFound();

			var playerViewModels = _mapper.Map<IEnumerable<PlayerViewModel>>(players);
			
			return Ok(playerViewModels);
        }

        // GET: api/Players/5
        [HttpGet("{id}")]
        public async Task<ActionResult<PlayerViewModel>> GetPlayer(int id)
        {
            var player = await _context.Players.FindAsync(id);

            if (player == null)
            {
                return NotFound();
            }

			var playerViewModel = _mapper.Map<PlayerViewModel>(player);

            return Ok(playerViewModel);
        }

		// PUT: api/Players/5
		// To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
		/*[HttpPut("{id}")]
		public async Task<IActionResult> PutPlayer(int id, PlayerUpdateViewModel playerUpdateViewModel)
		{

			var player = await _context.Players.FindAsync(id);

			if (player == null)
				return NotFound();

			_mapper.Map(playerUpdateViewModel, player);

			_context.Entry(player).State = EntityState.Modified;
			await _context.SaveChangesAsync();

			return NoContent();

			*//*if (id != player.PlayerId)
			{
				return BadRequest("Player ID mismatch.");
			}

			// Знаходимо існуючого гравця
			var existingPlayer = await _context.Players.FindAsync(id);
			if (existingPlayer == null)
			{
				return NotFound($"Player with ID {id} not found.");
			}

			// Оновлюємо поля існуючого гравця
			existingPlayer.Nickname = player.Nickname;
			existingPlayer.Password = player.Password;
			existingPlayer.Email = player.Email;
			existingPlayer.Sex = player.Sex;
			existingPlayer.RegistrationDate = player.RegistrationDate;
			existingPlayer.RegistrationTime = player.RegistrationTime;
			existingPlayer.Score = player.Score;

			// Оновлюємо сутність в базі даних
			_context.Entry(existingPlayer).State = EntityState.Modified;

			try
			{
				await _context.SaveChangesAsync();
			}
			catch (DbUpdateConcurrencyException)
			{
				if (!PlayerExists(id))
				{
					return NotFound($"Player with ID {id} not found.");
				}
				else
				{
					throw;
				}
			}

			return NoContent();*//*  // 204 OK - успішне оновлення
		}*/

		[HttpPut("{id}")]
		public async Task<IActionResult> PutPlayer(int id, PlayerUpdateViewModel playerUpdateViewModel)
		{
			if (id != playerUpdateViewModel.PlayerId)
			{
				return BadRequest("Player ID mismatch.");
			}

			if (await _context.Players.AnyAsync(p => p.Email == playerUpdateViewModel.Email && p.PlayerId != id))
			{
				return Conflict(new { message = "A player with this email already exists." });
			}

			var player = await _context.Players.FindAsync(id);
			if (player == null)
			{
				return NotFound();
			}

			_mapper.Map(playerUpdateViewModel, player);

			try
			{
				await _context.SaveChangesAsync();
			}
			catch (DbUpdateConcurrencyException)
			{
				if (!PlayerExists(id))
				{
					return NotFound();
				}
				else
				{
					throw;
				}
			}

			return NoContent();
		}

		// POST: api/Players
		// To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
		[HttpPost]
		public async Task<ActionResult<PlayerViewModel>> PostPlayer(PlayerCreateViewModel playerCreateViewModel)
		{
			var player = _mapper.Map<Player>(playerCreateViewModel);

			_context.Players.Add(player);

			await _context.SaveChangesAsync();

			var playerViewModel = _mapper.Map<PlayerViewModel>(player);

			return CreatedAtAction("GetPlayer", new { id = player.PlayerId }, player);

			/*var newPlayer = new Player();

			newPlayer.Nickname = player.Nickname;
			newPlayer.Password = player.Password;
			newPlayer.Email = player.Email;
			newPlayer.Sex = player.Sex;

			// Add the new player entity to the context
			_context.Players.Add(newPlayer);

			// Save the changes to the database
			await _context.SaveChangesAsync();

			// Return a response with the created player (excluding the ID which will be generated automatically)
			return CreatedAtAction("GetPlayer", new { id = newPlayer.PlayerId }, newPlayer);*/
		}


		// DELETE: api/Players/5
		[HttpDelete("{id}")]
        public async Task<IActionResult> DeletePlayer(int id)
        {
            var player = await _context.Players.FindAsync(id);
            
			if (player == null)
            {
                return NotFound();
            }

            _context.Players.Remove(player);
			try
			{
				await _context.SaveChangesAsync();
			}
			catch (Exception ex)
			{
				return BadRequest(ex.Message);
			}

			var playerViewModel = _mapper.Map<PlayerViewModel>(player);

			return Ok(playerViewModel);
        }

        private bool PlayerExists(int id)
        {
            return _context.Players.Any(e => e.PlayerId == id);
        }
    }
}

public abstract class PlayerRequest
{
	public string Nickname { get; set; } = null!;

	public string Password { get; set; } = null!;

	public string Email { get; set; } = null!;

	public string Sex { get; set; } = null!;

	public TimeOnly RegistrationTime { get; set; }

	public DateOnly RegistrationDate { get; set; }

	public int Score { get; set; }
}

public class PlayerRequestPut : PlayerRequest
{
	public int PlayerId { get; set; }
}

public class PlayerRequestPost : PlayerRequest { }

