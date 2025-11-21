using AutoMapper;
using FluentAssertions;
using Moq;
using VaccinationCard.Application.DTOs;
using VaccinationCard.Application.UseCases.Vaccinations.Commands.UpdateVaccination;
using VaccinationCard.Domain.Entities;
using VaccinationCard.Domain.Interfaces;
using Xunit;

namespace VaccinationCard.UnitTests.Application.UseCases.Vaccinations;

public class UpdateVaccinationHandlerTests
{
    private readonly Mock<IVaccinationRepository> _vaccinationRepoMock;
    private readonly Mock<IVaccineRepository> _vaccineRepoMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly UpdateVaccinationHandler _handler;

    public UpdateVaccinationHandlerTests()
    {
        _vaccinationRepoMock = new Mock<IVaccinationRepository>();
        _vaccineRepoMock = new Mock<IVaccineRepository>();
        _mapperMock = new Mock<IMapper>();
        _handler = new UpdateVaccinationHandler(_vaccinationRepoMock.Object, _vaccineRepoMock.Object, _mapperMock.Object);
    }

    [Fact]
    public async Task Handle_Should_Update_When_VaccineExists()
    {
        // ARRANGE
        var command = new UpdateVaccinationCommand(1, 2, "D2", DateTime.Now);
        
        var existingVaccination = new Vaccination(1, 1, "D1", DateTime.Now.AddDays(-10));
        var newVaccine = new Vaccine("Coronavac", 1); 

        _vaccinationRepoMock.Setup(r => r.GetByIdAsync(command.Id)).ReturnsAsync(existingVaccination);
        _vaccineRepoMock.Setup(r => r.GetByIdAsync(command.VaccineId)).ReturnsAsync(newVaccine);
        
        _mapperMock.Setup(m => m.Map<VaccinationDto>(It.IsAny<Vaccination>()))
            .Returns(new VaccinationDto());

        // ACT
        await _handler.Handle(command, CancellationToken.None);

        // ASSERT
        existingVaccination.Dose.Should().Be("D2");
        _vaccinationRepoMock.Verify(r => r.UpdateAsync(existingVaccination), Times.Once);
    }

    [Fact]
    public async Task Handle_Should_ThrowError_When_NewVaccineDoesNotExist()
    {
        // ARRANGE
        var command = new UpdateVaccinationCommand(1, 99, "D1", DateTime.Now);
        var existingVaccination = new Vaccination(1, 1, "D1", DateTime.Now);

        _vaccinationRepoMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(existingVaccination);
        _vaccineRepoMock.Setup(r => r.GetByIdAsync(99)).ReturnsAsync((Vaccine?)null);

        // ACT & ASSERT
        Func<Task> act = async () => await _handler.Handle(command, CancellationToken.None);
        
        await act.Should().ThrowAsync<Exception>()
            .WithMessage("*Vaccine not found*");
            
        _vaccinationRepoMock.Verify(r => r.UpdateAsync(It.IsAny<Vaccination>()), Times.Never);
    }
}