using System;

using TheaterService.DTOs;
using TheaterService.Models;

namespace TheaterService.Services;

public interface ITheaterServices {
	Task<List<Theater>> GetAll();
	Task<Theater?> GetOne(int id);
	Task<Theater> Create(NewTheater newTheater);
	Task<Theater?> Update(int id, NewTheater newTheater);
	Task Delete(int id);
}
