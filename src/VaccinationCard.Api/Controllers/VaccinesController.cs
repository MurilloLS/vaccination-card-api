using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using VaccinationCard.Application.UseCases.Vaccines.Commands.CreateVaccine;
using VaccinationCard.Application.UseCases.Vaccines.Queries.GetAllVaccines;

namespace VaccinationCard.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class VaccinesController : ControllerBase
{
    private readonly IMediator _mediator;

    public VaccinesController(IMediator mediator)
    {
        _mediator = mediator;
    }

    // POST
    [HttpPost]
    [Authorize(Roles = "ADMIN")] 
    [ProducesResponseType(StatusCodes.Status201Created)]
    public async Task<IActionResult> Create([FromBody] CreateVaccineCommand command)
    {
        var result = await _mediator.Send(command);
        return Created("", result);
    }

    // GET
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var query = new GetAllVaccinesQuery();
        var result = await _mediator.Send(query);
        return Ok(result);
    }
}