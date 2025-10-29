using System;

namespace TheaterService.DTOs;

public class MovieDto {
	public int Id { get; set; }
	public string Title { get; set; }
	public string Description { get; set; }
	public int Duration { get; set; }
	public float Rating { get; set; }
	public DateTime? ReleaseDate { get; set; }
	public bool Adult { get; set; }
}
