using MediatR;
using VaccinationCard.Application.DTOs;

namespace VaccinationCard.Application.UseCases.Auth.Commands.RegisterUser;

public record RegisterUserCommand(string Username, string Password) : IRequest<AuthResponseDto>;