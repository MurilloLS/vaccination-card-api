using AutoMapper;
using FluentAssertions;
using Moq;
using VaccinationCard.Application.DTOs;
using VaccinationCard.Application.UseCases.Vaccines.Commands.DeleteVaccine;
using VaccinationCard.Domain.Entities;
using VaccinationCard.Domain.Exceptions;
using VaccinationCard.Domain.Interfaces;
using Xunit;

namespace VaccinationCard.UnitTests.Application.UseCases.Vaccines;

public class DeleteVaccineHandlerTests
{
    private readonly Mock<IVaccineRepository> _repoMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly DeleteVaccineHandler _handler;

    public DeleteVaccineHandlerTests()
    {
        _repoMock = new Mock<IVaccineRepository>();
        _mapperMock = new Mock<IMapper>();
        _handler = new DeleteVaccineHandler(_repoMock.Object, _mapperMock.Object);
    }

    [Fact]
    public async Task Handle_Should_Delete_When_VaccineNotUsed()
    {
        // ARRANGE
        var vaccine = new Vaccine("Vacina Teste", 1, 5);
        typeof(Vaccine).GetProperty(nameof(Vaccine.Id))!.SetValue(vaccine, 1);
        
        _repoMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(vaccine);
        _repoMock.Setup(r => r.HasVaccinationAsync(1)).ReturnsAsync(false);
        
        _mapperMock.Setup(m => m.Map<VaccineDto>(vaccine)).Returns(new VaccineDto());

        // ACT
        var result = await _handler.Handle(new DeleteVaccineCommand(1), CancellationToken.None);

        // ASSERT
        result.Should().NotBeNull();
        _repoMock.Verify(r => r.DeleteAsync(vaccine), Times.Once);
    }

    [Fact]
    public async Task Handle_Should_ThrowException_When_VaccineInUse()
    {
        // ARRANGE
        var vaccine = new Vaccine("Vacina Importante", 1, 5);
        
        typeof(Vaccine).GetProperty(nameof(Vaccine.Id))!.SetValue(vaccine, 1);
        _repoMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(vaccine);
        _repoMock.Setup(r => r.HasVaccinationAsync(1)).ReturnsAsync(true);

        // ACT & ASSERT
        Func<Task> act = async () => await _handler.Handle(new DeleteVaccineCommand(1), CancellationToken.None);

        await act.Should().ThrowAsync<DomainException>()
            .WithMessage("*already been applied*");

        _repoMock.Verify(r => r.DeleteAsync(It.IsAny<Vaccine>()), Times.Never);
    }

    [Fact]
    public async Task Handle_Should_ReturnNull_When_NotFound()
    {
        _repoMock.Setup(r => r.GetByIdAsync(99)).ReturnsAsync((Vaccine?)null);
        
        var result = await _handler.Handle(new DeleteVaccineCommand(99), CancellationToken.None);
        
        result.Should().BeNull();
    }
}