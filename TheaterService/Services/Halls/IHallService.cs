using TheaterService.DTOs;
using TheaterService.Models;

namespace TheaterService.Services.Halls;

public interface IHallService {
	Task<ICollection<Hall>> GetAllHalls(int theater_id);
	Task<Hall?> GetHall(int id);
	Task<Hall> CreateHall(int theater_id, NewHall newHall);
	Task<Hall?> UpdateHall(int theater_id, int id, NewHall newHall);
	Task DeleteHall(int theater_id, int id);
	Task DeleteAllHalls(int theater_id);
}
