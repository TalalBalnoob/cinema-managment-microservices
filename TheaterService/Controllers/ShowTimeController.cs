using System.Text.Json;

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

using TheaterService.Data;
using TheaterService.DTOs;
using TheaterService.Models;

namespace TheaterService.Controllers {
	[Route("/theaters/{theater_id}/halls/{hall_id}/showtimes")]
	[ApiController]
	public class ShowTimeController(AppDbContext db) : ControllerBase {
		[HttpGet]
		public IActionResult GetShowTimesList([FromRoute] int hall_id, [FromQuery] DateOnly? date) {
			var showtimes = db.Showtimes.Where(s => s.Hall_Id == hall_id);
			if (date.HasValue) {
				showtimes = showtimes.Where(s => s.Date == date.Value);
			}
			return Ok(showtimes);
		}

		[HttpGet("/{id}")]
		public IActionResult GetShowTime([FromRoute] int hall_id, [FromRoute] int id) {
			var showtime = db.Showtimes.Find(id);
			if (showtime == null || showtime.Hall_Id != hall_id) return NotFound();
			return Ok(showtime);
		}

		[HttpPost]
		public async Task<IActionResult> CreateNewShowTime([FromRoute] int hall_id, [FromBody] NewShowTime newShowtime) {
			var hall = db.Halls.Find(hall_id);
			if (hall == null) return NotFound();

			// Get the movie details
			using var http = new HttpClient();
			var moviesRes = await http.GetAsync($"http://movie-service/{newShowtime.Movie_Id}");
			if (!moviesRes.IsSuccessStatusCode) return BadRequest("Movie Not found");

			// convert json res to movie object to use for validate data
			var movieJson = await moviesRes.Content.ReadAsStringAsync();
			var movie = JsonSerializer.Deserialize<MovieDto>(movieJson, new JsonSerializerOptions {
				PropertyNameCaseInsensitive = true
			});

			// Check if the movie duration align with show time duration
			var showtimeDuration = (newShowtime.End_time - newShowtime.Start_time).TotalMinutes;
			if (movie.Duration + 30 >= showtimeDuration) return BadRequest();

			// Check for overlap shows in the newShow duration
			var overlaps = await db.Showtimes
	   		.Where(s => s.Hall_Id == hall_id
		   	&& s.Date == newShowtime.Date
		   	&& (
			   (newShowtime.Start_time >= s.Start_time && newShowtime.Start_time < s.End_time)
			   || (newShowtime.End_time > s.Start_time && newShowtime.End_time <= s.End_time)
			   || (newShowtime.Start_time <= s.Start_time && newShowtime.End_time >= s.End_time)
		   	))
	   		.AnyAsync();

			if (overlaps)
				return Conflict("Another showtime is already scheduled in this hall during that period.");


			var showtime = db.Showtimes.Add(new Showtime {
				Id = 0,
				Movie_Id = newShowtime.Movie_Id,
				Hall_Id = hall_id,
				Price = newShowtime.Price,
				Date = newShowtime.Date,
				Start_time = newShowtime.Start_time,
				End_time = newShowtime.End_time,
			});

			db.SaveChanges();
			return CreatedAtAction(nameof(GetShowTime), new { id = showtime.Entity.Id, hall_id = hall_id }, showtime.Entity);
		}

	}
}
