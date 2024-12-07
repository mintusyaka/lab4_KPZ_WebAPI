using lab4_KPZ.Data;
using lab4_KPZ.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Collections;

namespace lab4_KPZ.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class PlayersCharactersController : ControllerBase
	{
        public readonly CharityGameContext _context;
        public PlayersCharactersController(CharityGameContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<PlayersCharacter>>> Get()
        {
            return await _context.PlayersCharacters.ToListAsync();
        }

		[HttpGet("filter/byPlayer/{playerId}")]
		public async Task<ActionResult<IEnumerable<PlayersCharacter>>> GetByPlayerId(int playerId)
		{
			var records = await _context.PlayersCharacters
				.Where(row => row.PlayerId == playerId)
				.ToListAsync();

			if (!records.Any())
			{
				return NotFound($"No characters found for player with ID {playerId}");
			}

			return Ok(records);
		}

		[HttpGet("filter/byCharacter/{characterId}")]
		public async Task<ActionResult<IEnumerable<PlayersCharacter>>> GetByCharacterId(int characterId)
		{
			return await _context.PlayersCharacters.Where(row => row.CharacterId == characterId).ToListAsync();
		}

		[HttpPost]
		public async Task<ActionResult<PlayersCharacterResponse>> PostPlayer(PlayersCharacterRequest pc)
		{
			var player = await _context.Players.FirstOrDefaultAsync(p => p.PlayerId == pc.PlayerId);
			var character = await _context.Characters.FirstOrDefaultAsync(c => c.CharacterId == pc.CharacterId);

			if (player == null)
			{
				return NotFound($"Player with ID {pc.PlayerId} not found.");
			}

			if (character == null)
			{
				return NotFound($"Character with ID {pc.CharacterId} not found.");
			}

			var existingRecord = await _context.PlayersCharacters
				.FirstOrDefaultAsync(pcExisting => pcExisting.PlayerId == pc.PlayerId && pcExisting.CharacterId == pc.CharacterId);

			if (existingRecord != null)
			{
				return Conflict("This Player-Character relationship already exists.");
			}

			try
			{
				var sql = "INSERT INTO players_characters (player_id, character_id) VALUES (@PlayerId, @CharacterId)";
					await _context.Database.ExecuteSqlRawAsync(sql, new[] {
					new SqlParameter("@PlayerId", pc.PlayerId),
					new SqlParameter("@CharacterId", pc.CharacterId)
				});

				var response = new PlayersCharacterResponse
				{
					PlayerId = pc.PlayerId,
					CharacterId = pc.CharacterId
				};

				return CreatedAtAction("GetByPlayerId", new { playerId = response.PlayerId }, response);
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex);
				return StatusCode(500, "An error occurred while processing your request.");
			}
		}


		private bool PlayerExists(int id)
		{
			return _context.Players.Any(e => e.PlayerId == id);
		}

		private bool CharacterExists(int id)
		{
			return _context.Characters.Any(c => c.CharacterId == id);
		}
	}
}

public class PlayersCharacterRequest
{
	public int PlayerId { get; set; }
	public int CharacterId { get; set; }
}

// DTO для відповіді
public class PlayersCharacterResponse
{
	public int PlayerId { get; set; }
	public int CharacterId { get; set; }
}
