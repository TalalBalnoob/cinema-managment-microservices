using MovieService.data;
using Microsoft.AspNetCore.Mvc;

namespace MovieService.Controllers;

[Route("/")]
[ApiController]
public class MoviesController(AppDbContext db) : ControllerBase {
	[HttpGet("health")]
	public IActionResult Health() {
		return Ok("Movie Service is healthy");
	}
	[HttpGet("/")]
	public IActionResult GetAll() {
		var moviesList = db.Movies.ToList();
		return Ok(moviesList);
	}
}
