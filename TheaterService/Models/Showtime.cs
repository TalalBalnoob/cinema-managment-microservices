using System;
using System.ComponentModel.DataAnnotations;

namespace TheaterService.Models;

public class Showtime {
	[Key]
	public int Id { get; set; }

	public int Movie_Id { get; set; }
	public int Hall_Id { get; set; }

	public float Price { get; set; }

	[Required]
	public DateOnly Date { get; set; }

	[Required]
	public TimeOnly Start_time { get; set; }
	[Required]
	public TimeOnly End_time { get; set; }

}
