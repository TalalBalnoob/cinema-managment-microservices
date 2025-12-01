using MediatR;

using TheaterService.Data;

namespace TheaterService.Services.SeatServices.DeleteAllSeats;


public class DeleteAllSeatsHandler : IRequestHandler<DeleteAllSeatsCommand>
{
	private readonly AppDbContext _db;

	public DeleteAllSeatsHandler(AppDbContext db)
	{
		_db = db;
	}

	public async Task Handle(DeleteAllSeatsCommand request, CancellationToken ct)
	{
		var seats = _db.Seats.Where(s => s.Hall_Id == request.HallId);
		_db.Seats.RemoveRange(seats);
		await _db.SaveChangesAsync(ct);
	}
}
