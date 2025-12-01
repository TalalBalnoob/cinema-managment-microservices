using System.Threading.Tasks;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

using TheaterService.Data;
using TheaterService.DTOs;
using TheaterService.Models;
using TheaterService.Services;
using TheaterService.Services.SeatServices;

namespace TheaterService.Controllers {
	[Route("/theaters/{theater_id}/halls/{hall_id}/seats")]
	[ApiController]
	public class SeatController(SeatService _service) : ControllerBase {
		[HttpGet]
		public async Task<IActionResult> GetSeatsList([FromRoute] int hall_id) {
			var seats = await _service.GetAllSeats(hall_id);

			return Ok(seats);
		}

		[HttpGet("/{id}")]
		public async Task<IActionResult> GetHall([FromRoute] int hall_id, [FromRoute] int id) {
			var seat = await _service.GetSeat(id);
			if (seat == null || seat.Hall_Id != hall_id) return NotFound();

			return Ok(seat);
		}

		[HttpPost]
		public async Task<IActionResult> CreateNewHall([FromRoute] int hall_id, [FromBody] NewSeat newSeat) {
			try {
				var seat = await _service.CreateSeat(hall_id, newSeat);
				return CreatedAtAction(nameof(GetHall), new { id = seat.Id, hall_id = seat.Hall_Id }, seat);
			}
			catch (System.Exception ex) {
				return BadRequest(new { message = ex.Message });
			}
		}

		[HttpPut("/{id}")]
		public async Task<IActionResult> UpdateHall([FromRoute] int hall_id, [FromRoute] int id, [FromBody] NewSeat newSeat) {
			try {
				var seat = await _service.UpdateSeat(hall_id, id, newSeat);
				return Ok(seat);
			}
			catch (System.Exception ex) {
				return BadRequest(new { message = ex.Message });
			}
		}

		[HttpDelete("/{id}")]
		public async Task<IActionResult> DeleteHall([FromRoute] int hall_id, [FromRoute] int id) {
			_service.DeleteSeat(hall_id, id);
			return NoContent();
		}

	}
}
