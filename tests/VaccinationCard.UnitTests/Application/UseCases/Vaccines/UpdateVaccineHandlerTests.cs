using AutoMapper;
using FluentAssertions;
using Moq;
using VaccinationCard.Application.DTOs;
using VaccinationCard.Application.UseCases.Vaccines.Commands.UpdateVaccine;
using VaccinationCard.Domain.Entities;
using VaccinationCard.Domain.Interfaces;
using Xunit;

namespace VaccinationCard.UnitTests.Application.UseCases.Vaccines;

public class UpdateVaccineHandlerTests
{
    private readonly Mock<IVaccineRepository> _repoMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly UpdateVaccineHandler _handler;

    public UpdateVaccineHandlerTests()
    {
        _repoMock = new Mock<IVaccineRepository>();
        _mapperMock = new Mock<IMapper>();
        _handler = new UpdateVaccineHandler(_repoMock.Object, _mapperMock.Object);
    }

    [Fact]
    public async Task Handle_Should_Update_When_VaccineExists()
    {
        // ARRANGE
        var command = new UpdateVaccineCommand(1, "Nome Editado", 1);
        var vaccine = new Vaccine("Nome Antigo", 1);

        _repoMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(vaccine);
        _mapperMock.Setup(m => m.Map<VaccineDto>(vaccine)).Returns(new VaccineDto());

        // ACT
        var result = await _handler.Handle(command, CancellationToken.None);

        // ASSERT
        result.Should().NotBeNull();
        vaccine.Name.Should().Be("Nome Editado"); // Verifica alteração na entidade
        _repoMock.Verify(r => r.UpdateAsync(vaccine), Times.Once);
    }

    [Fact]
    public async Task Handle_Should_ReturnNull_When_VaccineNotFound()
    {
        _repoMock.Setup(r => r.GetByIdAsync(99)).ReturnsAsync((Vaccine?)null);
        
        var result = await _handler.Handle(new UpdateVaccineCommand(99, "X", 1), CancellationToken.None);
        
        result.Should().BeNull();
        _repoMock.Verify(r => r.UpdateAsync(It.IsAny<Vaccine>()), Times.Never);
    }
}