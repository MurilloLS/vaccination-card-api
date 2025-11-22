using AutoMapper;
using FluentAssertions;
using Moq;
using VaccinationCard.Application.DTOs;
using VaccinationCard.Application.UseCases.Vaccines.Commands.CreateVaccine;
using VaccinationCard.Domain.Entities;
using VaccinationCard.Domain.Interfaces;
using Xunit;

namespace VaccinationCard.UnitTests.Application.UseCases.Vaccines;

public class CreateVaccineHandlerTests
{
    private readonly Mock<IVaccineRepository> _repoMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly CreateVaccineHandler _handler;

    public CreateVaccineHandlerTests()
    {
        _repoMock = new Mock<IVaccineRepository>();
        _mapperMock = new Mock<IMapper>();
        _handler = new CreateVaccineHandler(_repoMock.Object, _mapperMock.Object);
    }

    [Fact]
    public async Task Handle_Should_CreateVaccine_When_Valid()
    {
        // ARRANGE
        var command = new CreateVaccineCommand("Nova Vacina", 1);
        
        _mapperMock.Setup(m => m.Map<VaccineDto>(It.IsAny<Vaccine>()))
            .Returns(new VaccineDto { Id = 1, Name = "Nova Vacina" });

        // ACT
        var result = await _handler.Handle(command, CancellationToken.None);

        // ASSERT
        result.Should().NotBeNull();
        _repoMock.Verify(r => r.AddAsync(It.IsAny<Vaccine>()), Times.Once);
    }
}