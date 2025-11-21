using MediatR;
using Microsoft.AspNetCore.Mvc;
using VaccinationCard.Application.UseCases.Vaccinations.Commands.CreateVaccination;

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
}