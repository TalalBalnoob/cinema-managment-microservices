using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using TheaterService.Data;
using TheaterService.DTOs;
using TheaterService.Models;
using TheaterService.Services;

namespace TheaterService.Controllers {
	[Route("theaters/")]
	[ApiController]
	public class TheaterController : ControllerBase {
		private readonly AppDbContext db;
		private readonly TheaterServices _service;

		public TheaterController(AppDbContext dbContext, TheaterServices theaterServices) {
			db = dbContext;
			_service = theaterServices;
		}

		[HttpGet("health")]
		public IActionResult GetStatus() {
			return Ok(new { status = "Theater service is running." });
		}

		[HttpGet]
		public IActionResult GetTheaters() {
			var theaters = db.Theaters.ToList();
			return Ok(theaters);
		}

		[HttpGet("/{id}")]
		public IActionResult GetTheater(int id) {
			var theater = db.Theaters.Find(id);
			if (theater == null) {
				return NotFound(new { message = "Theater not found." });
			}
			return Ok(theater);
		}

		[HttpGet("/{id}/halls")]
		public IActionResult GetHallsForTheater(int id) {
			var halls = _service.GetHallsForTheater(id);
			return Ok(halls);
		}


		[HttpPost]
		public IActionResult CreateTheater([FromBody] NewTheater newTheater) {
			var theater = db.Theaters.Add(new Theater {
				Id = 0,
				Name = newTheater.Name,
				Location = newTheater.Location
			});

			db.SaveChanges();
			return CreatedAtAction(nameof(GetTheater), new { id = theater.Entity.Id }, theater.Entity);
		}

		[HttpPut("/{id}")]
		public IActionResult UpdateTheater(int id, [FromBody] NewTheater newTheater) {
			var theaterFromDb = db.Theaters.Find(id);
			if (theaterFromDb == null) return NotFound();

			theaterFromDb.Name = newTheater.Name;
			theaterFromDb.Location = newTheater.Location;

			db.SaveChanges();
			return Ok(theaterFromDb);
		}

		[HttpDelete("/{id}")]
		public IActionResult DeleteTheater(int id) {
			var theaterFromDb = db.Theaters.Find(id);
			if (theaterFromDb == null) return NotFound();

			db.Theaters.Remove(theaterFromDb);
			return NoContent();
		}
	}
}
