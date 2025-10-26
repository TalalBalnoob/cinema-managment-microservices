using MovieService.data;
using Microsoft.AspNetCore.Mvc;
using MovieService.Models;
using Microsoft.AspNetCore.Authorization;

namespace MovieService.Controllers;

[Route("/")]
[ApiController]
public class MoviesController(AppDbContext db) : ControllerBase {
	[HttpGet("health")]
	[AllowAnonymous]
	public IActionResult Health() {
		return Ok("Movie Service is healthy");
	}
	[HttpGet("/")]
	[AllowAnonymous]
	public IActionResult GetAll() {
		var moviesList = db.Movies.ToList();
		return Ok(moviesList);
	}

	[HttpGet("/{id}")]
	[AllowAnonymous]
	public IActionResult GetById(int id) {
		var movie = db.Movies.Find(id);
		if (movie == null) return NotFound();
		return Ok(movie);
	}

	[HttpPost("/")]
	[Authorize]
	public IActionResult CreateMovie(Movies movie) {
		var newMovie = new Movies {
			Id = 0,
			Title = movie.Title,
			Description = movie.Description ?? string.Empty,
			Duration = movie.Duration,
			Adult = movie.Adult,
			ReleaseDate = movie.ReleaseDate ?? null,
			Rating = movie.Rating
		};

		db.Movies.Add(newMovie);
		db.SaveChangesAsync();
		return CreatedAtAction(nameof(GetAll), new { id = newMovie.Id }, newMovie);

	}

	[Authorize]
	[HttpPut("/{id}")]
	public IActionResult UpdateMovie(int id, Movies updatedMovie) {
		var existingMovie = db.Movies.Find(id);
		if (existingMovie == null) return NotFound();

		existingMovie.Title = updatedMovie.Title;
		existingMovie.Description = updatedMovie.Description;
		existingMovie.Duration = updatedMovie.Duration;
		existingMovie.Rating = updatedMovie.Rating;
		existingMovie.Adult = updatedMovie.Adult;
		existingMovie.ReleaseDate = updatedMovie.ReleaseDate;

		db.SaveChangesAsync();
		return NoContent();
	}

	[Authorize]
	[HttpDelete("/{id}")]
	public IActionResult DeleteMovie(int id) {
		var movie = db.Movies.Find(id);
		if (movie == null) {
			return NotFound();
		}
		db.Movies.Remove(movie);
		db.SaveChangesAsync();
		return NoContent();
	}
}
