using System;
using System.ComponentModel.DataAnnotations;

namespace TheaterService.Models;

public class Theater {
	[Key]
	public int Id { get; set; }

	[Required]
	public string Name { get; set; }

	public string Location { get; set; }
}
