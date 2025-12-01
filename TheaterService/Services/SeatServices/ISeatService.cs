using TheaterService.DTOs;
using TheaterService.Models;

namespace TheaterService.Services.SeatServices;

public interface ISeatService {
	Task<IEnumerable<Seat>> GetAllSeats(int hall_id);
	Task<Seat?> GetSeat(int id);
	Task<IEnumerable<Seat>> GetSeats(List<int> ids);
	Task<Seat> CreateSeat(int hall_id, NewSeat newSeat);
	Task<List<Seat>> GenerateAllSeatsInHall(int hall_id);
	Task<Seat?> UpdateSeat(int hall_id, int id, NewSeat newSeat);
	void DeleteSeat(int hall_id, int id);
	void DeleteAllSeat(int hall_id);
}
