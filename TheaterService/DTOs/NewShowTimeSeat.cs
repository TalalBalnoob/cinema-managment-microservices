using System;

namespace TheaterService.DTOs;

public class NewShowTimeSeat
{
	public int Booking_Id { get; set; }
	public int Seat_Id { get; set; }
	public string Status { get; set; } = "AVAILABLE";
}
