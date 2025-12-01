using MediatR;

using Microsoft.EntityFrameworkCore;

using TheaterService.Data;
using TheaterService.DTOs;
using TheaterService.Models;
using TheaterService.Services.Halls;
using TheaterService.Services.SeatServices;

namespace TheaterService.Services;

public class SeatService(AppDbContext db, IMediator mediator) : ISeatService {
	public async Task<IEnumerable<Seat>> GetAllSeats(int hall_id) {
		var seats = db.Seats.Where(s => s.Hall_Id == hall_id);

		return seats;
	}

	public async Task<Seat?> GetSeat(int id) {
		var seat = db.Seats.Find(id);

		return seat;
	}

	public async Task<IEnumerable<Seat>> GetSeats(List<int> ids) {
		var seats = await db.Seats.Where(s => ids.Contains(s.Id)).ToListAsync();

		return seats;
	}

	public async Task<Seat> CreateSeat(int hall_id, NewSeat newSeat) {
		var hall = db.Halls.Find(hall_id);
		if (hall == null) throw new Exception("Hall not found!");

		if (newSeat.Number >= hall.Layout_columns) {
			throw new Exception($"Seat number must be less than the number of columns in the hall ({hall.Layout_columns}).");
		}

		if (newSeat.Row >= hall.Layout_rows) {
			throw new Exception($"Seat row number must be less than the number of rows in the hall ({hall.Layout_rows}).");
		}

		var seat = db.Seats.Add(new Seat {
			Id = 0,
			Number = newSeat.Number,
			Row = newSeat.Row,
			Hall_Id = hall_id,
			IsActive = newSeat.IsActive,
			Seat_type = newSeat.Seat_type,
		});

		db.SaveChanges();
		return seat.Entity;
	}

	public async Task<List<Seat>> GenerateAllSeatsInHall(int hall_id) {
		var hall = db.Halls.FirstOrDefault(h => h.Id == hall_id);

		this.DeleteAllSeat(hall_id);

		var seats = new List<Seat>();

		for (int c = 0; c < hall.Layout_columns; c++) {
			for (int r = 0; r < hall.Layout_rows; r++) {
				var seat = new NewSeat {
					Hall_Id = hall_id,
					Number = c,
					Row = r,
					IsActive = true,
					Seat_type = "standard",
				};
				var createdSeat = await this.CreateSeat(hall_id, seat);
				seats.Add(createdSeat);
			}
		}

		return seats;
	}

	public async Task<Seat?> UpdateSeat(int hall_id, int id, NewSeat newSeat) {
		var seat = db.Seats.Include(s => s.Hall).Single(s => s.Id == id || s.Hall_Id == hall_id);
		if (seat == null) throw new Exception("Seat not found!");
		if (seat.Hall == null) throw new Exception("Hall not found!");


		if (newSeat.Number >= seat.Hall.Layout_columns) {
			throw new Exception($"Seat number must be less than the number of columns in the hall ({seat.Hall.Layout_columns}).");
		}

		if (newSeat.Row >= seat.Hall.Layout_rows) {
			throw new Exception($"Seat row number must be less than the number of rows in the hall ({seat.Hall.Layout_rows}).");
		}

		seat.Number = newSeat.Number;
		seat.Row = newSeat.Row;
		seat.IsActive = newSeat.IsActive;
		seat.Seat_type = newSeat.Seat_type;

		db.SaveChanges();

		return seat;
	}

	public void DeleteSeat(int hall_id, int id) {
		var seat = db.Seats.Find(id);
		if (seat == null || seat.Hall_Id != hall_id) throw new Exception("Seat not found");

		db.Seats.Remove(seat);
		db.SaveChanges();
	}

	public async void DeleteAllSeat(int hall_id) {
		var seats = db.Seats.Where(s => s.Hall_Id == hall_id);

		db.Seats.RemoveRange(seats);
	}
}
