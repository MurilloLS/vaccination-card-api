using FluentAssertions;
using Moq;
using VaccinationCard.Application.Common.Interfaces;
using VaccinationCard.Application.DTOs;
using VaccinationCard.Application.UseCases.Auth.Commands.RegisterUser;
using VaccinationCard.Domain.Entities;
using VaccinationCard.Domain.Exceptions;
using VaccinationCard.Domain.Interfaces;
using Xunit;

namespace VaccinationCard.UnitTests.Application.UseCases.Auth;

public class RegisterUserHandlerTests
{
    private readonly Mock<IUserRepository> _userRepoMock;
    private readonly Mock<IPasswordHasher> _hasherMock;
    private readonly Mock<ITokenService> _tokenServiceMock;
    private readonly RegisterUserHandler _handler;

    public RegisterUserHandlerTests()
    {
        _userRepoMock = new Mock<IUserRepository>();
        _hasherMock = new Mock<IPasswordHasher>();
        _tokenServiceMock = new Mock<ITokenService>();

        _handler = new RegisterUserHandler(
            _userRepoMock.Object,
            _hasherMock.Object,
            _tokenServiceMock.Object
        );
    }

    [Fact]
    public async Task Handle_Should_RegisterUser_When_UsernameIsUnique()
    {
        // ARRANGE
        var command = new RegisterUserCommand("murillo_dev", "senha123");

        _userRepoMock.Setup(r => r.GetByUsernameAsync(command.Username))
            .ReturnsAsync((User?)null);
        _hasherMock.Setup(h => h.Hash(command.Password)).Returns("hash_xyz");
        _tokenServiceMock.Setup(t => t.GenerateToken(It.IsAny<User>())).Returns("jwt_token_fake");

        // ACT
        var result = await _handler.Handle(command, CancellationToken.None);

        // ASSERT
        result.Should().NotBeNull();
        result.Token.Should().Be("jwt_token_fake");
        result.Username.Should().Be("murillo_dev");
        
        _hasherMock.Verify(h => h.Hash("senha123"), Times.Once);
        _userRepoMock.Verify(r => r.AddAsync(It.IsAny<User>()), Times.Once);
    }

    [Fact]
    public async Task Handle_Should_ThrowException_When_UsernameAlreadyExists()
    {
        // ARRANGE
        var command = new RegisterUserCommand("murillo_dev", "senha123");
        var existingUser = new User("murillo_dev", "hash", "USER");

        _userRepoMock.Setup(r => r.GetByUsernameAsync(command.Username))
            .ReturnsAsync(existingUser);

        // ACT & ASSERT
        Func<Task> act = async () => await _handler.Handle(command, CancellationToken.None);

        await act.Should().ThrowAsync<DomainException>()
            .WithMessage("*already exists*");

        _userRepoMock.Verify(r => r.AddAsync(It.IsAny<User>()), Times.Never);
    }
}