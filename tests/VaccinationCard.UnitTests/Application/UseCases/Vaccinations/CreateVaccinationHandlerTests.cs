using AutoMapper;
using FluentAssertions;
using Moq;
using VaccinationCard.Application.DTOs;
using VaccinationCard.Application.UseCases.Vaccinations.Commands.CreateVaccination;
using VaccinationCard.Domain.Entities;
using VaccinationCard.Domain.Interfaces;
using Xunit; // Importante para o [Fact]

namespace VaccinationCard.UnitTests.Application.UseCases.Vaccinations;

public class CreateVaccinationHandlerTests
{
    // Mocks
    private readonly Mock<IVaccinationRepository> _vaccinationRepoMock;
    private readonly Mock<IPersonRepository> _personRepoMock;
    private readonly Mock<IVaccineRepository> _vaccineRepoMock;
    private readonly Mock<IMapper> _mapperMock;

    // O Handler real que será testado
    private readonly CreateVaccinationHandler _handler;

    public CreateVaccinationHandlerTests()
    {
        // Inicializa os Mocks
        _vaccinationRepoMock = new Mock<IVaccinationRepository>();
        _personRepoMock = new Mock<IPersonRepository>();
        _vaccineRepoMock = new Mock<IVaccineRepository>();
        _mapperMock = new Mock<IMapper>();

        // Instancia o Handler injetando os mocks no lugar dos repositórios reais
        _handler = new CreateVaccinationHandler(
            _vaccinationRepoMock.Object,
            _personRepoMock.Object,
            _vaccineRepoMock.Object,
            _mapperMock.Object
        );
    }

    [Fact] // Indica que é um teste
    public async Task Handle_Should_CreateVaccination_When_DataIsValid()
    {
        // ARRANGE (Preparação do Cenário)
        var command = new CreateVaccinationCommand(1, 1, "D1", DateTime.Now);

        var person = new Person("Murillo", 30, "M"); 
        var vaccine = new Vaccine("BCG", 1);         
        
        // Configura o Mock
        _personRepoMock.Setup(r => r.GetByIdAsync(command.PersonId))
            .ReturnsAsync(person);
        _vaccineRepoMock.Setup(r => r.GetByIdAsync(command.VaccineId))
            .ReturnsAsync(vaccine);

        // Configura o Mock do Mapper
        _mapperMock.Setup(m => m.Map<VaccinationDto>(It.IsAny<Vaccination>()))
            .Returns(new VaccinationDto { Id = 10, PersonName = "Murillo", VaccineName = "BCG" });

        // ACT 
        var result = await _handler.Handle(command, CancellationToken.None);

        // ASSERT
        result.Should().NotBeNull(); 
        
        // Verifica se o método AddAsync foi chamado EXATAMENTE 1 vez.
        _vaccinationRepoMock.Verify(r => r.AddAsync(It.IsAny<Vaccination>()), Times.Once);
    }

    [Fact]
    public async Task Handle_Should_ThrowError_When_PersonNotFound()
    {
        // ARRANGE
        var command = new CreateVaccinationCommand(99, 1, "D1", DateTime.Now);

        // Configura o Mock
        _personRepoMock.Setup(r => r.GetByIdAsync(command.PersonId))
            .ReturnsAsync((Person?)null);

        // ACT
        Func<Task> act = async () => await _handler.Handle(command, CancellationToken.None);

        // ASSERT
        await act.Should().ThrowAsync<Exception>()
            .WithMessage("*Person not found*"); 
            
        // Verifica se o sistema NUNCA tentou salvar
        _vaccinationRepoMock.Verify(r => r.AddAsync(It.IsAny<Vaccination>()), Times.Never);
    }
}