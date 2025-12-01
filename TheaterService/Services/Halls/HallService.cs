using MediatR;

using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;

using TheaterService.Data;
using TheaterService.DTOs;
using TheaterService.Models;
using TheaterService.Services.Halls.DeleteHall;
using TheaterService.Services.SeatServices;
using TheaterService.Services.SeatServices.DeleteAllSeats;

namespace TheaterService.Services.Halls;

public class HallService(AppDbContext db, IMediator mediator) : IHallService {
	public async Task<ICollection<Hall>> GetAllHalls(int theater_id) {
		var halls = db.Halls
			.Include(h => h.Seats)
			.Where(h => h.Theater_Id == theater_id).ToList();

		return halls;
	}

	public async Task<Hall?> GetHall(int id) {
		var hall = db.Halls.Include(h => h.Seats).FirstOrDefault(h => h.Id == id);
		if (hall == null) return null;

		return hall;
	}

	public async Task<Hall> CreateHall(int theater_id, NewHall newHall) {
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

	public async Task<Hall?> UpdateHall(int theater_id, int id, NewHall newHall) {
		var hall = db.Halls.FirstOrDefault(h => h.Id == id && h.Theater_Id == theater_id);
		if (hall == null) return null;

		hall.Name = newHall.Name;
		hall.Layout_columns = newHall.Layout_columns;
		hall.Layout_rows = newHall.Layout_rows;
		hall.Capacity = newHall.Layout_columns * newHall.Layout_rows;

		db.SaveChanges();
		return hall;
	}

	public async Task DeleteHall(int theater_id, int id) {
		await mediator.Send(new DeleteHallCommand(theater_id, id));
		return;
	}

	public async Task DeleteAllHalls(int theater_id) {
		var halls = db.Halls.Where(h => h.Theater_Id == theater_id);
		if (halls == null) return;

		foreach (var hall in halls) {
			await this.DeleteHall(theater_id, hall.Id);
		}

		return;
	}
}
