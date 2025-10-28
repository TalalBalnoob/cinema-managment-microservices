using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace TheaterService.Models;

public class Hall {
	[Key]
	public int Id { get; set; }

	[Required]
	public int Theater_Id { get; set; }

	[Required]
	public string Name { get; set; }

	[Required]
	public int Capacity { get; set; }

	[Required]
	public int Layout_rows { get; set; }

	[Required]
	public int Layout_columns { get; set; }

	[ForeignKey("Theater_Id")]
	public Theater? Theater { get; set; }

}
