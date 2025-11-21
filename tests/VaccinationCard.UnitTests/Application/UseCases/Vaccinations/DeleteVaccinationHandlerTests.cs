using AutoMapper;
using FluentAssertions;
using Moq;
using VaccinationCard.Application.DTOs;
using VaccinationCard.Application.UseCases.Vaccinations.Commands.DeleteVaccination;
using VaccinationCard.Domain.Entities;
using VaccinationCard.Domain.Interfaces;
using Xunit;

namespace VaccinationCard.UnitTests.Application.UseCases.Vaccinations;

public class DeleteVaccinationHandlerTests
{
    private readonly Mock<IVaccinationRepository> _repoMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly DeleteVaccinationHandler _handler;

    public DeleteVaccinationHandlerTests()
    {
        _repoMock = new Mock<IVaccinationRepository>();
        _mapperMock = new Mock<IMapper>();
        _handler = new DeleteVaccinationHandler(_repoMock.Object, _mapperMock.Object);
    }

    [Fact]
    public async Task Handle_Should_Delete_When_Exists()
    {
        var vaccination = new Vaccination(1, 1, "D1", DateTime.Now);
        _repoMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(vaccination);
        _mapperMock.Setup(m => m.Map<VaccinationDto>(vaccination)).Returns(new VaccinationDto());

        var result = await _handler.Handle(new DeleteVaccinationCommand(1), CancellationToken.None);

        result.Should().NotBeNull();
        _repoMock.Verify(r => r.DeleteAsync(vaccination), Times.Once);
    }
}