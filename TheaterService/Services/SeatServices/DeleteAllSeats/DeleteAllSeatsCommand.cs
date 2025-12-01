using MediatR;

namespace TheaterService.Services.SeatServices.DeleteAllSeats;

public record DeleteAllSeatsCommand(int HallId) : IRequest;
