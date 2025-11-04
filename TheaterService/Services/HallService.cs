using System;

using Microsoft.EntityFrameworkCore;

using TheaterService.Data;
using TheaterService.DTOs;
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

	public Hall CreateHall(int theater_id, NewHall newHall) {
		var hall = db.Halls.Add(new Hall {
			Id = 0,
			Name = newHall.Name,
			Layout_columns = newHall.Layout_columns,
			Layout_rows = newHall.Layout_rows,
			Capacity = newHall.Layout_rows * newHall.Layout_columns,
			Theater_Id = theater_id
		});

		db.SaveChanges();
		return hall.Entity;
	}

	public Hall UpdateHall(int theater_id, int id, NewHall newHall) {
		var hall = db.Halls.FirstOrDefault(h => h.Id == id && h.Theater_Id == theater_id);
		if (hall == null) throw new Exception("Hall not found");

		hall.Name = newHall.Name;
		hall.Layout_columns = newHall.Layout_columns;
		hall.Layout_rows = newHall.Layout_rows;
		hall.Capacity = newHall.Layout_columns * newHall.Layout_rows;

		db.SaveChanges();
		return hall;
	}

	public void DeleteHall(int theater_id, int id) {
		var hall = db.Halls.FirstOrDefault(h => h.Id == id && h.Theater_Id == theater_id);
		if (hall == null) throw new Exception("Hall not found");

		var showsInHall = db.Showtimes.Any(s => s.Hall_Id == id);
		if (showsInHall) throw new Exception("Cannot delete hall with scheduled showtimes");

		var seats = db.Seats.Where(s => s.Hall_Id == id);

		db.Seats.RemoveRange(seats);
		db.Halls.Remove(hall);
		db.SaveChanges();
		return;
	}
}
