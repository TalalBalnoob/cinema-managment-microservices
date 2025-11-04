using System;

using Microsoft.EntityFrameworkCore;

using TheaterService.Data;
using TheaterService.Models;

namespace TheaterService.Services;

public class HallService(AppDbContext db) {
	public ICollection<Hall> GetAllHalls(int theater_id) {
		var halls = db.Halls
			.Include(h => h.Seats)
			.Where(h => h.Theater_Id == theater_id).ToList();

		return halls;
	}

	public Hall GetHall(int id) {
		var hall = db.Halls.Include(h => h.Seats).FirstOrDefault(h => h.Id == id);
		if (hall == null) throw new Exception("Hall not found");

		return hall;
	}
}
