using AutoMapper;
using FluentAssertions;
using Moq;
using VaccinationCard.Application.DTOs;
using VaccinationCard.Application.UseCases.Persons.Queries.GetPersonCard;
using VaccinationCard.Domain.Entities;
using VaccinationCard.Domain.Interfaces;
using Xunit;

namespace VaccinationCard.UnitTests.Application.UseCases.Persons;

public class GetPersonCardHandlerTests
{
    private readonly Mock<IPersonRepository> _personRepoMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly GetPersonCardHandler _handler;

    public GetPersonCardHandlerTests()
    {
        _personRepoMock = new Mock<IPersonRepository>();
        _mapperMock = new Mock<IMapper>();
        _handler = new GetPersonCardHandler(_personRepoMock.Object, _mapperMock.Object);
    }

    [Fact]
    public async Task Handle_Should_ReturnPersonCard_When_Found()
    {
        // ARRANGE
        var person = new Person("Murillo", 30, "M");
        _personRepoMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(person);
        _mapperMock.Setup(m => m.Map<PersonCardDto>(person)).Returns(new PersonCardDto { Id = 1, Name = "Murillo" });

        // ACT
        var result = await _handler.Handle(new GetPersonCardQuery(1), CancellationToken.None);

        // ASSERT
        result.Should().NotBeNull();
        result!.Name.Should().Be("Murillo");
    }

    [Fact]
    public async Task Handle_Should_ReturnNull_When_NotFound()
    {
        _personRepoMock.Setup(r => r.GetByIdAsync(99)).ReturnsAsync((Person?)null);
        var result = await _handler.Handle(new GetPersonCardQuery(99), CancellationToken.None);
        result.Should().BeNull();
    }
}