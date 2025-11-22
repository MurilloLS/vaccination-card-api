using FluentAssertions;
using Moq;
using VaccinationCard.Application.Common.Interfaces;
using VaccinationCard.Application.UseCases.Auth.Commands.LoginUser;
using VaccinationCard.Domain.Entities;
using VaccinationCard.Domain.Exceptions;
using VaccinationCard.Domain.Interfaces;
using Xunit;

namespace VaccinationCard.UnitTests.Application.UseCases.Auth;

public class LoginUserHandlerTests
{
    private readonly Mock<IUserRepository> _userRepoMock;
    private readonly Mock<IPasswordHasher> _hasherMock;
    private readonly Mock<ITokenService> _tokenServiceMock;
    private readonly LoginUserHandler _handler;

    public LoginUserHandlerTests()
    {
        _userRepoMock = new Mock<IUserRepository>();
        _hasherMock = new Mock<IPasswordHasher>();
        _tokenServiceMock = new Mock<ITokenService>();

        _handler = new LoginUserHandler(
            _userRepoMock.Object,
            _hasherMock.Object,
            _tokenServiceMock.Object
        );
    }

    [Fact]
    public async Task Handle_Should_ReturnToken_When_CredentialsAreValid()
    {
        // ARRANGE
        var command = new LoginUserCommand("admin", "admin123");
        var userInDb = new User("admin", "hashed_secret", "ADMIN");

        _userRepoMock.Setup(r => r.GetByUsernameAsync(command.Username)).ReturnsAsync(userInDb);
        _hasherMock.Setup(h => h.Verify(command.Password, userInDb.PasswordHash)).Returns(true);
        _tokenServiceMock.Setup(t => t.GenerateToken(userInDb)).Returns("valid_token");

        // ACT
        var result = await _handler.Handle(command, CancellationToken.None);

        // ASSERT
        result.Token.Should().Be("valid_token");
    }

    [Fact]
    public async Task Handle_Should_ThrowError_When_UserNotFound()
    {
        // ARRANGE
        var command = new LoginUserCommand("ghost", "123");
        _userRepoMock.Setup(r => r.GetByUsernameAsync(command.Username)).ReturnsAsync((User?)null);

        // ACT & ASSERT
        Func<Task> act = async () => await _handler.Handle(command, CancellationToken.None);

        await act.Should().ThrowAsync<DomainException>()
            .WithMessage("Invalid username or password.");
    }

    [Fact]
    public async Task Handle_Should_ThrowError_When_PasswordIsWrong()
    {
        // ARRANGE
        var command = new LoginUserCommand("admin", "wrong_pass");
        var userInDb = new User("admin", "hashed_secret", "ADMIN");

        _userRepoMock.Setup(r => r.GetByUsernameAsync(command.Username)).ReturnsAsync(userInDb);
        
        // Senha NÃO bate (Verify retorna false)
        _hasherMock.Setup(h => h.Verify(command.Password, userInDb.PasswordHash)).Returns(false);

        // ACT & ASSERT
        Func<Task> act = async () => await _handler.Handle(command, CancellationToken.None);

        await act.Should().ThrowAsync<DomainException>()
            .WithMessage("Invalid username or password.");
    }
}