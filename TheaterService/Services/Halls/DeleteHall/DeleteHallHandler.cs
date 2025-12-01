using MediatR;

using Microsoft.EntityFrameworkCore;

using TheaterService.Data;
using TheaterService.Services.SeatServices.DeleteAllSeats;

namespace TheaterService.Services.Halls.DeleteHall;

public class DeleteHallHandler : IRequestHandler<DeleteHallCommand>
{
	private readonly AppDbContext _db;
	private readonly IMediator _mediator;

	public DeleteHallHandler(AppDbContext db, IMediator mediator)
	{
		_db = db;
		_mediator = mediator;
	}

	public async Task Handle(DeleteHallCommand request, CancellationToken ct)
	{
		var hall = await _db.Halls
			.FirstOrDefaultAsync(h => h.Id == request.HallId && h.Theater_Id == request.TheaterId);

		if (hall == null) throw new Exception("Hall not found");

		var showsInHall = await _db.Showtimes
			.AnyAsync(s => s.Hall_Id == request.HallId);

		if (showsInHall)
			throw new Exception("Cannot delete hall with scheduled showtimes");

		await _mediator.Send(new DeleteAllSeatsCommand(request.HallId));

		_db.Halls.Remove(hall);
		await _db.SaveChangesAsync(ct);
	}
}
