using AutoMapper;
using FluentAssertions;
using Moq;
using VaccinationCard.Application.DTOs;
using VaccinationCard.Application.UseCases.Persons.Commands.DeletePerson;
using VaccinationCard.Domain.Entities;
using VaccinationCard.Domain.Interfaces;
using Xunit;

namespace VaccinationCard.UnitTests.Application.UseCases.Persons;

public class DeletePersonHandlerTests
{
    private readonly Mock<IPersonRepository> _personRepoMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly DeletePersonHandler _handler;

    public DeletePersonHandlerTests()
    {
        _personRepoMock = new Mock<IPersonRepository>();
        _mapperMock = new Mock<IMapper>();
        _handler = new DeletePersonHandler(_personRepoMock.Object, _mapperMock.Object);
    }

    [Fact]
    public async Task Handle_Should_DeletePerson_When_Exists()
    {
        // ARRANGE
        var command = new DeletePersonCommand(1);
        var person = new Person("Murillo", 30, "M");

        _personRepoMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(person);
        _mapperMock.Setup(m => m.Map<PersonDto>(person)).Returns(new PersonDto { Id = 1, Name = "Murillo" });

        // ACT
        var result = await _handler.Handle(command, CancellationToken.None);

        // ASSERT
        result.Should().NotBeNull();
        _personRepoMock.Verify(r => r.DeleteAsync(person), Times.Once);
    }

    [Fact]
    public async Task Handle_Should_ReturnNull_When_PersonNotFound()
    {
        // ARRANGE
        _personRepoMock.Setup(r => r.GetByIdAsync(99)).ReturnsAsync((Person?)null);

        // ACT
        var result = await _handler.Handle(new DeletePersonCommand(99), CancellationToken.None);

        // ASSERT
        result.Should().BeNull();
        _personRepoMock.Verify(r => r.DeleteAsync(It.IsAny<Person>()), Times.Never);
    }
}