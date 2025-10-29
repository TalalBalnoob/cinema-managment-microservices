using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

using TheaterService.Data;
using TheaterService.DTOs;
using TheaterService.Models;

namespace TheaterService.Controllers {
	[Route("/theaters/{theater_id}/halls/{hall_id}/seats")]
	[ApiController]
	public class SeatController(AppDbContext db) : ControllerBase {
		[HttpGet]
		public IActionResult GetSeatsList([FromRoute] int hall_id) {
			var seats = db.Seats.Where(s => s.Hall_Id == hall_id);

			return Ok(seats);
		}

		[HttpGet("/{id}")]
		public IActionResult GetHall([FromRoute] int hall_id, [FromRoute] int id) {
			var seat = db.Seats.Find(id);
			if (seat == null || seat.Hall_Id != hall_id) return NotFound();

			return Ok(seat);
		}

		[HttpPost]
		public IActionResult CreateNewHall([FromRoute] int hall_id, [FromBody] NewSeat newSeat) {
			var hall = db.Halls.Find(hall_id);
			if (hall == null) return NotFound();

			if (newSeat.Number >= hall.Layout_columns) {
				return BadRequest($"Seat number must be less than the number of columns in the hall ({hall.Layout_columns}).");
			}

			if (newSeat.Row >= hall.Layout_rows) {
				return BadRequest($"Seat row number must be less than the number of rows in the hall ({hall.Layout_rows}).");
			}

			var seat = db.Seats.Add(new Seat {
				Id = 0,
				Number = newSeat.Number,
				Row = newSeat.Row,
				Hall_Id = hall_id,
				IsActive = newSeat.IsActive,
				Seat_type = newSeat.Seat_type,
			});

			db.SaveChanges();
			return CreatedAtAction(nameof(GetHall), new { id = seat.Entity.Id, hall_id = seat.Entity.Hall_Id }, seat.Entity);
		}

		[HttpPut("/{id}")]
		public IActionResult UpdateHall([FromRoute] int hall_id, [FromRoute] int id, [FromBody] NewSeat newSeat) {
			var seat = db.Seats.Include(s => s.Hall).Single(s => s.Id == id || s.Hall_Id == hall_id);
			if (seat == null) return NotFound();

			if (newSeat.Number >= seat.Hall.Layout_columns) {
				return BadRequest($"Seat number must be less than the number of columns in the hall ({seat.Hall.Layout_columns}).");
			}

			if (newSeat.Row >= seat.Hall.Layout_rows) {
				return BadRequest($"Seat row number must be less than the number of rows in the hall ({seat.Hall.Layout_rows}).");
			}

			seat.Number = newSeat.Number;
			seat.Row = newSeat.Row;
			seat.IsActive = newSeat.IsActive;
			seat.Seat_type = newSeat.Seat_type;

			db.SaveChanges();
			return Ok(seat);
		}

		[HttpDelete("/{id}")]
		public IActionResult DeleteHall([FromRoute] int hall_id, [FromRoute] int id) {
			var seat = db.Seats.Find(id);
			if (seat == null || seat.Hall_Id != hall_id) return NotFound();

			db.Seats.Remove(seat);
			db.SaveChanges();
			return NoContent();
		}

	}
}
