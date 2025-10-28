using System;

using Microsoft.EntityFrameworkCore;

using TheaterService.Models;

namespace TheaterService.Data;

public class AppDbContext : DbContext {
	public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

	public DbSet<Theater> Theaters { get; set; }
	public DbSet<Seat> Seats { get; set; }
	public DbSet<Hall> Halls { get; set; }
	public DbSet<Showtime> Showtimes { get; set; }
	public DbSet<ShowTimeSeat> ShowTimeSeats { get; set; }
}
