using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

using TheaterService.Data;
using TheaterService.DTOs;
using TheaterService.Models;
using TheaterService.Statics;
using TheaterService.Services;

namespace TheaterService.Controllers
{
    [Route("/showtime")]
    [ApiController]
    public class ShowTimeSeatController(AppDbContext db, TheaterServices _service) : ControllerBase
    {
        [HttpGet("{showTime_id}/seats")]
        public IActionResult GetShowTimeSeats(int showTime_id)
        {
            var showTimeSeats = db
                .ShowTimeSeats.Include(sts => sts.ShowTime)
                .Where(sts => sts.Showtime_Id == showTime_id)
                .ToList();

            if (showTimeSeats.Count == 0)
            {
                return NotFound(new { message = "No seats found for the specified show time." });
            }

            return Ok(showTimeSeats);
        }

        [HttpGet("{showTime_id}/seats/{seat_id}")]
        public IActionResult GetShowTimeSeat(int showTime_id, int seat_id)
        {
            var showTimeSeat = db
                .ShowTimeSeats.Include(sts => sts.ShowTime)
                .FirstOrDefault(sts => sts.Showtime_Id == showTime_id && sts.Seat_Id == seat_id);

            if (showTimeSeat == null)
            {
                return NotFound(new { message = "Seat not found for the specified show time." });
            }

            return Ok(showTimeSeat);
        }

        // make all the seats for a showtime
        [HttpPost("{showtime_id}/seats/generate")]
        public IActionResult CreateShowTimeSeat(
            int showtime_id,
            [FromBody] NewShowTimeSeat newShowTimeSeat
        )
        {
            var showTime = db.Showtimes.FirstOrDefault(st => st.Id == showtime_id);

            if (showTime == null)
            {
                return NotFound(new { message = "Show time not found." });
            }

            db.ShowTimeSeats.Add(
                new ShowTimeSeat
                {
                    Id = 0,
                    Booking_Id = newShowTimeSeat.Booking_Id,
                    Seat_Id = newShowTimeSeat.Seat_Id,
                    Showtime_Id = showtime_id,
                    Status = ShowTimeSeat_Status.AVAILABLE,
                }
            );
            db.SaveChanges();

            return CreatedAtAction(nameof(GetShowTimeSeat), new
            {
                showTime_id = showtime_id,
                seat_id = newShowTimeSeat.Seat_Id,
            },
                newShowTimeSeat
            );
        }
    }
}
