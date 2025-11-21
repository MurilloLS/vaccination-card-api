using AutoMapper;
using FluentAssertions;
using Moq;
using VaccinationCard.Application.DTOs;
using VaccinationCard.Application.UseCases.Vaccinations.Queries.GetVaccinationById;
using VaccinationCard.Domain.Entities;
using VaccinationCard.Domain.Interfaces;
using Xunit;

namespace VaccinationCard.UnitTests.Application.UseCases.Vaccinations;

public class GetVaccinationByIdHandlerTests
{
    private readonly Mock<IVaccinationRepository> _repoMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly GetVaccinationByIdHandler _handler;

    public GetVaccinationByIdHandlerTests()
    {
        _repoMock = new Mock<IVaccinationRepository>();
        _mapperMock = new Mock<IMapper>();
        _handler = new GetVaccinationByIdHandler(_repoMock.Object, _mapperMock.Object);
    }

    [Fact]
    public async Task Handle_Should_ReturnVaccination_When_Found()
    {
        // ARRANGE
        var vaccination = new Vaccination(1, 1, "D1", DateTime.Now);
        var dto = new VaccinationDto { Id = 1, Dose = "D1" };

        _repoMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(vaccination);
        _mapperMock.Setup(m => m.Map<VaccinationDto>(vaccination)).Returns(dto);

        // ACT
        var result = await _handler.Handle(new GetVaccinationByIdQuery(1), CancellationToken.None);

        // ASSERT
        result.Should().NotBeNull();
        result!.Dose.Should().Be("D1");
        _repoMock.Verify(r => r.GetByIdAsync(1), Times.Once);
    }

    [Fact]
    public async Task Handle_Should_ReturnNull_When_NotFound()
    {
        // ARRANGE
        _repoMock.Setup(r => r.GetByIdAsync(99)).ReturnsAsync((Vaccination?)null);

        // ACT
        var result = await _handler.Handle(new GetVaccinationByIdQuery(99), CancellationToken.None);

        // ASSERT
        result.Should().BeNull();
    }
}