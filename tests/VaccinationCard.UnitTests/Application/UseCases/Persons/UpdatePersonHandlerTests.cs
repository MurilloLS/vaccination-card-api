using AutoMapper;
using FluentAssertions;
using Moq;
using VaccinationCard.Application.DTOs;
using VaccinationCard.Application.UseCases.Persons.Commands.UpdatePerson;
using VaccinationCard.Domain.Entities;
using VaccinationCard.Domain.Interfaces;
using Xunit;

namespace VaccinationCard.UnitTests.Application.UseCases.Persons;

public class UpdatePersonHandlerTests
{
    private readonly Mock<IPersonRepository> _personRepoMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly UpdatePersonHandler _handler;

    public UpdatePersonHandlerTests()
    {
        _personRepoMock = new Mock<IPersonRepository>();
        _mapperMock = new Mock<IMapper>();
        _handler = new UpdatePersonHandler(_personRepoMock.Object, _mapperMock.Object);
    }

    [Fact]
    public async Task Handle_Should_UpdatePerson_When_PersonExists()
    {
        // ARRANGE
        var command = new UpdatePersonCommand(1, "Murillo Updated", 31, "M");
        var person = new Person("Murillo Old", 30, "M"); // Entidade original

        // Mock: Encontra a pessoa
        _personRepoMock.Setup(r => r.GetByIdAsync(command.Id)).ReturnsAsync(person);
        
        // Mock: Retorna o DTO atualizado
        _mapperMock.Setup(m => m.Map<PersonDto>(It.IsAny<Person>()))
            .Returns(new PersonDto { Id = 1, Name = "Murillo Updated" });

        // ACT
        var result = await _handler.Handle(command, CancellationToken.None);

        // ASSERT
        result.Should().NotBeNull();
        person.Name.Should().Be("Murillo Updated"); // Verifica se a entidade mudou na memória
        
        // Verifica se chamou o UpdateAsync no banco
        _personRepoMock.Verify(r => r.UpdateAsync(person), Times.Once);
    }

    [Fact]
    public async Task Handle_Should_ReturnNull_When_PersonNotFound()
    {
        // ARRANGE
        var command = new UpdatePersonCommand(99, "Ghost", 20, "M");
        _personRepoMock.Setup(r => r.GetByIdAsync(99)).ReturnsAsync((Person?)null);

        // ACT
        var result = await _handler.Handle(command, CancellationToken.None);

        // ASSERT
        result.Should().BeNull();
        _personRepoMock.Verify(r => r.UpdateAsync(It.IsAny<Person>()), Times.Never);
    }
}