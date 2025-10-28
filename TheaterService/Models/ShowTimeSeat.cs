using System;
using System.ComponentModel.DataAnnotations;

using TheaterService.Statics;

namespace TheaterService.Models;

public class ShowTimeSeat {
	[Key]
	public int Id { get; set; }

	[Required]
	public int Showtime_Id { get; set; }
	[Required]
	public int Seat_Id { get; set; }
	public int Booking_Id { get; set; }

	public DateTime? Reserved_at { get; set; }

	public DateTime? Reservation_expires_at { get; set; }

	public string Status { get; set; } = ShowTimeSeat_Status.AVAILABLE;

}
