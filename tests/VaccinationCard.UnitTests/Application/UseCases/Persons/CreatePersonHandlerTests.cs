using AutoMapper;
using FluentAssertions;
using Moq;
using VaccinationCard.Application.DTOs;
using VaccinationCard.Application.UseCases.Persons.Commands.CreatePerson;
using VaccinationCard.Domain.Entities;
using VaccinationCard.Domain.Interfaces;
using Xunit;

namespace VaccinationCard.UnitTests.Application.UseCases.Persons;

public class CreatePersonHandlerTests
{
    private readonly Mock<IPersonRepository> _personRepoMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly CreatePersonHandler _handler;

    public CreatePersonHandlerTests()
    {
        _personRepoMock = new Mock<IPersonRepository>();
        _mapperMock = new Mock<IMapper>();

        _handler = new CreatePersonHandler(_personRepoMock.Object, _mapperMock.Object);
    }

    [Fact]
    public async Task Handle_Should_CreatePerson_When_DataIsValid()
    {
        // ARRANGE
        var command = new CreatePersonCommand("Ana Paula", 17, "F");
        
        // Configura o Mapper para retornar um DTO válido
        _mapperMock.Setup(m => m.Map<PersonDto>(It.IsAny<Person>()))
            .Returns(new PersonDto { Id = 1, Name = "Ana Paula", Age = 17, Gender = "F" });

        // ACT
        var result = await _handler.Handle(command, CancellationToken.None);

        // ASSERT
        result.Should().NotBeNull();
        result.Name.Should().Be("Ana Paula");

        // Verifica se o método AddAsync foi chamado no repositório
        _personRepoMock.Verify(r => r.AddAsync(It.IsAny<Person>()), Times.Once);
    }
}