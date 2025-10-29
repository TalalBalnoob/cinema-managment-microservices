using System;

using TheaterService.Models;
using TheaterService.Statics;

namespace TheaterService.DTOs;

public class NewSeat {
	public string Seat_type { get; set; } = Seat_types.REGULAR;
	public int Row { get; set; }
	public int Number { get; set; }
	public bool IsActive { get; set; } = true;
	public int Hall_Id { get; set; }
}
