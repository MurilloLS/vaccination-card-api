using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MediatR;
using VaccinationCard.Application.UseCases.Vaccines.Queries.GetAllVaccines;
using VaccinationCard.Application.UseCases.Vaccines.Commands.CreateVaccine;
using VaccinationCard.Application.UseCases.Vaccines.Commands.UpdateVaccine;
using VaccinationCard.Application.UseCases.Vaccines.Commands.DeleteVaccine;
using VaccinationCard.Application.DTOs;

namespace VaccinationCard.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class VaccinesController : ControllerBase
{
    private readonly IMediator _mediator;

    public VaccinesController(IMediator mediator)
    {
        _mediator = mediator;
    }

    // GET: api/Vaccines
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var result = await _mediator.Send(new GetAllVaccinesQuery());
        return Ok(result);
    }

    // POST: api/Vaccines
    [HttpPost]
    [Authorize(Roles = "ADMIN")]
    public async Task<IActionResult> Create([FromBody] CreateVaccineRequest request)
    {
        var command = new CreateVaccineCommand(request.Name, request.CategoryId, request.MaxDoses);
        var response = await _mediator.Send(command);
        
        return Ok(response);
    }

    // PUT: api/Vaccines/5
    [HttpPut("{id}")]
    [Authorize(Roles = "ADMIN")]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateVaccineRequest request)
    {
        var command = new UpdateVaccineCommand(id, request.Name, request.CategoryId, request.MaxDoses);
        await _mediator.Send(command);
        return Ok(new { message = "Vaccine updated successfully." });
    }

    // DELETE: api/Vaccines/5
    [HttpDelete("{id}")]
    [Authorize(Roles = "ADMIN")]
    public async Task<IActionResult> Remove(int id)
    {
        await _mediator.Send(new DeleteVaccineCommand(id));
        return Ok(new { message = "Vaccine deleted successfully." });
    }
}