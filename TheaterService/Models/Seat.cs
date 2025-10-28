using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

using TheaterService.Statics;

namespace TheaterService.Models;

public class Seat {
	[Key]
	public int Id { get; set; }

	[Required]
	public int Hall_Id { get; set; }

	[Required]
	[Range(0, 100)]
	public int Row { get; set; }

	[Required]
	[Range(0, 100)]
	public int Number { get; set; }

	public string Seat_type { get; set; } = Seat_types.REGULAR;

	public bool isActive { get; set; } = true;

	[ForeignKey("Hall_Id")]
	public Hall? Hall { get; set; }
}
