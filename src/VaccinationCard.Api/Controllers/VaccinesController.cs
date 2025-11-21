using MediatR;
using Microsoft.AspNetCore.Mvc;
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

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var query = new GetAllVaccinesQuery();
        var result = await _mediator.Send(query);
        return Ok(result);
    }
}