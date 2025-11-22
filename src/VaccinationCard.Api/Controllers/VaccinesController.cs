using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using VaccinationCard.Application.UseCases.Vaccines.Commands.CreateVaccine;
using VaccinationCard.Application.UseCases.Vaccines.Queries.GetAllVaccines;
using VaccinationCard.Application.UseCases.Vaccines.Commands.UpdateVaccine;
using VaccinationCard.Application.UseCases.Vaccines.Commands.DeleteVaccine;
using VaccinationCard.Application.DTOs;

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

    // PUT
    [HttpPut("{id}")]
    [Authorize(Roles = "ADMIN")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateVaccineRequest request)
    {
        var command = new UpdateVaccineCommand(id, request.Name, request.CategoryId);

        var result = await _mediator.Send(command);
        
        if (result == null) return NotFound();
        
        return Ok(result);
    }

    // DELETE
    [HttpDelete("{id}")]
    [Authorize(Roles = "ADMIN")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(int id)
    {
        var command = new DeleteVaccineCommand(id);
        var result = await _mediator.Send(command);

        if (result == null) return NotFound();

        return Ok(result);
    }
}