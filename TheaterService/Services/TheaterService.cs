using System;

using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;

using TheaterService.Data;
using TheaterService.DTOs;
using TheaterService.Models;

namespace TheaterService.Services;

public class TheaterServices(AppDbContext _db) {
	public List<Theater> GetAll() {
		var theaters = _db.Theaters.ToList();

		return theaters;
	}

	public Theater GetOne(int id) {
		var theater = _db.Theaters.Include(t => t.Halls).SingleOrDefault(t => t.Id == id);

		return theater;
	}

	public Theater Create(NewTheater newTheater) {
		var theater = _db.Theaters.Add(new Theater {
			Id = 0,
			Name = newTheater.Name,
			Location = newTheater.Location
		});

		_db.SaveChanges();
		return theater.Entity;
	}

	public Theater Update(int id, NewTheater newTheater) {
		var theaterFromDb = _db.Theaters.SingleOrDefault(t => t.Id == id);
		if (theaterFromDb == null) ;

		theaterFromDb.Name = newTheater.Name;
		theaterFromDb.Location = newTheater.Location;

		_db.SaveChanges();
		return theaterFromDb;
	}

}
