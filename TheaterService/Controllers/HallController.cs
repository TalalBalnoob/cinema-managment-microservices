using System.Threading.Tasks;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using TheaterService.Data;
using TheaterService.DTOs;
using TheaterService.Models;
using TheaterService.Services;
using TheaterService.Services.Halls;

namespace TheaterService.Controllers {
	[Route("/theaters/{theater_id}/halls")]
	[ApiController]
	public class HallController(HallService _service) : ControllerBase {


		[HttpGet]
		public async Task<IActionResult> GetHallsInTheater([FromRoute] int theater_id) {
			var halls = await _service.GetAllHalls(theater_id);

			return Ok(halls);
		}

		[HttpGet("/{id}")]
		public async Task<IActionResult> GetHall([FromRoute] int theater_id, [FromRoute] int id) {
			var hall = await _service.GetHall(id);
			if (hall == null || hall.Theater_Id != theater_id) return NotFound();

			return Ok(hall);
		}

		[HttpPost]
		public async Task<IActionResult> CreateNewHall([FromRoute] int theater_id, [FromBody] NewHall newHall) {
			var hall = await _service.CreateHall(theater_id, newHall);
			return CreatedAtAction(nameof(GetHall), new { id = hall.Id, theater_id = hall.Theater_Id }, hall);
		}

		[HttpPut("/{id}")]
		public async Task<IActionResult> UpdateHall([FromRoute] int theater_id, [FromRoute] int id, [FromBody] NewHall newHall) {
			var hall = await _service.UpdateHall(theater_id, id, newHall);
			if (hall == null) return NotFound();
			return Ok(hall);
		}

		[HttpDelete("/{id}")]
		public IActionResult DeleteHall([FromRoute] int theater_id, [FromRoute] int id) {
			try {
				_service.DeleteHall(theater_id, id);
			}
			catch (System.Exception ex) {
				// return the error message
				return BadRequest(new { message = ex.Message });
			}
			return NoContent();
		}

	}
}
