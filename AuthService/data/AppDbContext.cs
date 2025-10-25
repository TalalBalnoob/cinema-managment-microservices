using System;

using AuthService.Models;

using Microsoft.EntityFrameworkCore;

namespace AuthService.data;

public class AppDbContext : DbContext {
	public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

	public DbSet<User> Users { get; set; }
}
