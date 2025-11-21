using MediatR;
using VaccinationCard.Application.Common.Interfaces;
using VaccinationCard.Application.DTOs;
using VaccinationCard.Domain.Exceptions;
using VaccinationCard.Domain.Interfaces;

namespace VaccinationCard.Application.UseCases.Auth.Commands.LoginUser;

public class LoginUserHandler : IRequestHandler<LoginUserCommand, AuthResponseDto>
{
    private readonly IUserRepository _userRepository;
    private readonly IPasswordHasher _hasher;
    private readonly ITokenService _tokenService;

    public LoginUserHandler(IUserRepository userRepository, IPasswordHasher hasher, ITokenService tokenService)
    {
        _userRepository = userRepository;
        _hasher = hasher;
        _tokenService = tokenService;
    }

    public async Task<AuthResponseDto> Handle(LoginUserCommand request, CancellationToken cancellationToken)
    {
        // 1. Busca usuário
        var user = await _userRepository.GetByUsernameAsync(request.Username);
        
        // Dica de segurança: Mensagem genérica evita enumeração de usuários
        if (user == null || !_hasher.Verify(request.Password, user.PasswordHash))
        {
            throw new DomainException("Invalid username or password.");
        }

        // 2. Gera Token
        var token = _tokenService.GenerateToken(user);

        return new AuthResponseDto(user.Id, user.Username, token);
    }
}