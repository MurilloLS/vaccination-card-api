using MediatR;
using Microsoft.AspNetCore.Mvc;
using VaccinationCard.Application.DTOs;
using VaccinationCard.Application.UseCases.Vaccinations.Commands.CreateVaccination;
using VaccinationCard.Application.UseCases.Vaccinations.Commands.DeleteVaccination;
using VaccinationCard.Application.UseCases.Vaccinations.Commands.UpdateVaccination;
using VaccinationCard.Application.UseCases.Vaccinations.Queries.GetVaccinationById;

namespace VaccinationCard.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class VaccinationsController : ControllerBase
{
    private readonly IMediator _mediator;

    public VaccinationsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create([FromBody] CreateVaccinationCommand command)
    {
        var response = await _mediator.Send(command);
        return Created("", response);
    }

    // GET By Id
    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(int id)
    {
        var query = new GetVaccinationByIdQuery(id);
        var result = await _mediator.Send(query);

        if (result == null) return NotFound(new { message = "Vaccination record not found" });

        return Ok(result);
    }

    // PUT (Update)
    [HttpPut("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateVaccinationRequest request)
    {
        var command = new UpdateVaccinationCommand(id, request.VaccineId, request.Dose, request.ApplicationDate);
        var result = await _mediator.Send(command);

        if (result == null) return NotFound(new { message = "Vaccination record not found" });

        return Ok(result);
    }

    // DELETE
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(int id)
    {
        var command = new DeleteVaccinationCommand(id);
        var result = await _mediator.Send(command);

        if (result == null) return NotFound(new { message = "Vaccination record not found" });

        return Ok(result);
    }
}