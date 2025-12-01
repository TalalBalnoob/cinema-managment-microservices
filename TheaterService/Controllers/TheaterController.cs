using Microsoft.AspNetCore.Mvc;

using TheaterService.DTOs;
using TheaterService.Services;

namespace TheaterService.Controllers {
	[Route("/")]
	[ApiController]
	public class TheaterController(TheaterServices _service) : ControllerBase {

		// private readonly ITheaterService _service;

		// public TheaterController(TheaterServices service) {
		// 	_service = service;
		// }

		[HttpGet("ping")]
		public IActionResult GetStatus() {
			return Ok(new { status = "Theater service is running." });
		}

		[HttpGet]
		public async Task<IActionResult> GetTheaters() {
			var theaters = await _service.GetAll();
			return Ok(theaters);
		}

		[HttpGet("/{id}")]
		public async Task<IActionResult> GetTheater(int id) {
			var theater = await _service.GetOne(id);
			if (theater == null) {
				return NotFound(new { message = "Theater not found." });
			}
			return Ok(theater);
		}


		[HttpPost]
		public async Task<IActionResult> CreateTheater([FromBody] NewTheater newTheater) {
			var theater = await _service.Create(newTheater);
			return CreatedAtAction(nameof(GetTheater), new { id = theater.Id }, theater);
		}

		[HttpPut("/{id}")]
		public async Task<IActionResult> UpdateTheater(int id, [FromBody] NewTheater newTheater) {
			var updatedTheater = await _service.Update(id, newTheater);
			if (updatedTheater == null) return NotFound();
			return Ok(updatedTheater);
		}

		[HttpDelete("/{id}")]
		public async Task<IActionResult> DeleteTheater(int id) {
			await _service.Delete(id);
			return NoContent();
		}
	}
}
