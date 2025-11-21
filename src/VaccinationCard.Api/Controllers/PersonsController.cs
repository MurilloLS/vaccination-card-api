using MediatR;
using Microsoft.AspNetCore.Mvc;
using VaccinationCard.Application.UseCases.Persons.Commands.CreatePerson;
using VaccinationCard.Application.UseCases.Persons.Queries.GetPersonCard;

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

    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(int id)
    {
        var query = new GetPersonCardQuery(id);
        var result = await _mediator.Send(query);

        if (result == null)
        {
            return NotFound(new { message = "Person not found" });
        }

        // Se achou, retorna 200 OK com o DTO
        return Ok(result);
    }
}