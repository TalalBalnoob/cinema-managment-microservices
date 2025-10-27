using System;

using Microsoft.EntityFrameworkCore;

namespace TheaterService.Data;

public class AppDbContext : DbContext {
	AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
}
