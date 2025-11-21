using MediatR;
using Microsoft.AspNetCore.Mvc;
using VaccinationCard.Application.UseCases.Persons.Commands.CreatePerson;

namespace VaccinationCard.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PersonsController : ControllerBase
{
    private readonly IMediator _mediator;

    public PersonsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create([FromBody] CreatePersonCommand command)
    {
        var response = await _mediator.Send(command);

        // Retorna 201 Created com o corpo da resposta
        return CreatedAtAction(nameof(Create), new { id = response.Id }, response);
    }
}