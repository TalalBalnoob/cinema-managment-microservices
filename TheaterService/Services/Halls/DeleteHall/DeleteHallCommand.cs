using MediatR;

namespace TheaterService.Services.Halls.DeleteHall;

public record DeleteHallCommand(int TheaterId, int HallId) : IRequest;
