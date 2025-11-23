using AutoMapper;
using FluentAssertions;
using Moq;
using VaccinationCard.Application.DTOs;
using VaccinationCard.Application.UseCases.Vaccines.Queries.GetAllVaccines;
using VaccinationCard.Domain.Entities;
using VaccinationCard.Domain.Interfaces;
using Xunit;

namespace VaccinationCard.UnitTests.Application.UseCases.Vaccines;

public class GetAllVaccinesHandlerTests
{
    private readonly Mock<IVaccineRepository> _repoMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly GetAllVaccinesHandler _handler;

    public GetAllVaccinesHandlerTests()
    {
        _repoMock = new Mock<IVaccineRepository>();
        _mapperMock = new Mock<IMapper>();
        _handler = new GetAllVaccinesHandler(_repoMock.Object, _mapperMock.Object);
    }

    [Fact]
    public async Task Handle_Should_ReturnAllVaccines()
    {
        // ARRANGE
        var vaccines = new List<Vaccine> { new Vaccine("BCG", 1, 1) };
        
        _repoMock.Setup(r => r.GetAllAsync()).ReturnsAsync(vaccines);
        _mapperMock.Setup(m => m.Map<IEnumerable<VaccineDto>>(vaccines))
            .Returns(new List<VaccineDto> { new VaccineDto { Name = "BCG" } });

        // ACT
        var result = await _handler.Handle(new GetAllVaccinesQuery(), CancellationToken.None);

        // ASSERT
        result.Should().HaveCount(1);
        _repoMock.Verify(r => r.GetAllAsync(), Times.Once);
    }
}