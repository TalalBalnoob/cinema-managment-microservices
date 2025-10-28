using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using TheaterService.Data;
using TheaterService.DTOs;
using TheaterService.Models;

namespace TheaterService.Controllers {
	[Route("/theaters/{theater_id}/halls")]
	[ApiController]
	public class HallController(AppDbContext db) : ControllerBase {
		[HttpGet]
		public IActionResult GetHallsInTheater([FromRoute] int theater_id) {
			var halls = db.Halls.Where(h => h.Theater_Id == theater_id);

			return Ok(halls);
		}

		[HttpGet("/{id}")]
		public IActionResult GetHall([FromRoute] int theater_id, [FromRoute] int id) {
			var hall = db.Halls.Find(id);
			if (hall == null || hall.Theater_Id != theater_id) return NotFound();

			return Ok(hall);
		}

		[HttpPost]
		public IActionResult CreateNewHall([FromRoute] int theater_id, [FromBody] NewHall newHall) {
			var hall = db.Halls.Add(new Hall {
				Id = 0,
				Name = newHall.Name,
				Layout_columns = newHall.Layout_columns,
				Layout_rows = newHall.Layout_rows,
				Capacity = newHall.Layout_rows * newHall.Layout_columns,
				Theater_Id = theater_id
			});

			db.SaveChanges();
			return CreatedAtAction(nameof(GetHall), new { id = hall.Entity.Id, theater_id = hall.Entity.Theater_Id }, hall.Entity);
		}

		[HttpPut("/{id}")]
		public IActionResult UpdateHall([FromRoute] int theater_id, [FromRoute] int id, [FromBody] NewHall newHall) {
			var hall = db.Halls.Find(id);
			if (hall == null || hall.Theater_Id != theater_id) return NotFound();

			hall.Name = newHall.Name;
			hall.Layout_columns = newHall.Layout_columns;
			hall.Layout_rows = newHall.Layout_rows;
			hall.Capacity = newHall.Layout_columns * newHall.Layout_rows;

			db.SaveChanges();
			return Ok(hall);
		}

		[HttpDelete("/{id}")]
		public IActionResult DeleteHall([FromRoute] int theater_id, [FromRoute] int id) {
			var hall = db.Halls.Find(id);
			if (hall == null || hall.Theater_Id != theater_id) return NotFound();

			db.Halls.Remove(hall);
			db.SaveChanges();
			return NoContent();
		}

	}
}
