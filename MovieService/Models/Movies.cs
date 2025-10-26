using System.ComponentModel.DataAnnotations;

namespace MovieService.Models;

public class Movies {
	[Key]
	public int Id { get; set; }

	[Required]
	public string Title { get; set; } = string.Empty;

	public string Description { get; set; } = string.Empty;

	[Required]
	public int Duration { get; set; }

	[Range(0.0, 5.0)]
	public float Rating { get; set; }
}
