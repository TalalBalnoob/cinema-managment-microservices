using MovieService.Models;

using Microsoft.EntityFrameworkCore;

namespace MovieService.data;

public class AppDbContext : DbContext {
	public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

	public DbSet<Movies> Movies { get; set; }
}
