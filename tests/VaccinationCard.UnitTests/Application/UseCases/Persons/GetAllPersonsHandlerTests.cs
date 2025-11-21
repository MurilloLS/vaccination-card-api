using AutoMapper;
using FluentAssertions;
using Moq;
using VaccinationCard.Application.DTOs;
using VaccinationCard.Application.UseCases.Persons.Queries.GetAllPersons;
using VaccinationCard.Domain.Entities;
using VaccinationCard.Domain.Interfaces;
using Xunit;

namespace VaccinationCard.UnitTests.Application.UseCases.Persons;

public class GetAllPersonsHandlerTests
{
    private readonly Mock<IPersonRepository> _repoMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly GetAllPersonsHandler _handler;

    public GetAllPersonsHandlerTests()
    {
        _repoMock = new Mock<IPersonRepository>();
        _mapperMock = new Mock<IMapper>();
        _handler = new GetAllPersonsHandler(_repoMock.Object, _mapperMock.Object);
    }

    [Fact]
    public async Task Handle_Should_ReturnList_When_Called()
    {
        // ARRANGE
        var persons = new List<Person> 
        { 
            new Person("Ana", 20, "F"), 
            new Person("Beto", 30, "M") 
        };

        // Mock do Repositório retornando a lista
        _repoMock.Setup(r => r.GetAllAsync()).ReturnsAsync(persons);
        
        // Mock do Mapper retornando lista de DTOs
        _mapperMock.Setup(m => m.Map<IEnumerable<PersonDto>>(persons))
            .Returns(new List<PersonDto> 
            { 
                new PersonDto { Name = "Ana" }, 
                new PersonDto { Name = "Beto" } 
            });

        // ACT
        var result = await _handler.Handle(new GetAllPersonsQuery(), CancellationToken.None);

        // ASSERT
        result.Should().HaveCount(2);
        _repoMock.Verify(r => r.GetAllAsync(), Times.Once);
    }
}