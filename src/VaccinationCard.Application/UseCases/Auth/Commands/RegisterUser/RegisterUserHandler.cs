using MediatR;
using VaccinationCard.Application.Common.Interfaces;
using VaccinationCard.Application.DTOs;
using VaccinationCard.Domain.Entities;
using VaccinationCard.Domain.Exceptions;
using VaccinationCard.Domain.Interfaces;

namespace VaccinationCard.Application.UseCases.Auth.Commands.RegisterUser;

public class RegisterUserHandler : IRequestHandler<RegisterUserCommand, AuthResponseDto>
{
    private readonly IUserRepository _userRepository;
    private readonly IPasswordHasher _hasher;
    private readonly ITokenService _tokenService;

    public RegisterUserHandler(IUserRepository userRepository, IPasswordHasher hasher, ITokenService tokenService)
    {
        _userRepository = userRepository;
        _hasher = hasher;
        _tokenService = tokenService;
    }

    public async Task<AuthResponseDto> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
    {
        // 1. Verifica duplicidade
        var existingUser = await _userRepository.GetByUsernameAsync(request.Username);
        DomainException.When(existingUser != null, "Username already exists.");

        // 2. Cria usuário com senha HASHEADA
        var passwordHash = _hasher.Hash(request.Password);
        var user = new User(request.Username, passwordHash, "USER"); // Role padrão

        await _userRepository.AddAsync(user);

        // 3. Gera Token já no cadastro (UX melhor)
        var token = _tokenService.GenerateToken(user);

        return new AuthResponseDto(user.Id, user.Username, token);
    }
}