using System;

namespace TheaterService.DTOs;

public class NewShowTime {
	public int Movie_Id { get; set; }
	public float Price { get; set; }
	public DateOnly Date { get; set; }
	public TimeOnly Start_time { get; set; }
	public TimeOnly End_time { get; set; }

}
