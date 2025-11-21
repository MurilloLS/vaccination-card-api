using MediatR;
using VaccinationCard.Application.DTOs;

namespace VaccinationCard.Application.UseCases.Auth.Commands.LoginUser;

public record LoginUserCommand(string Username, string Password) : IRequest<AuthResponseDto>;