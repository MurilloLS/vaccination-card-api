using MediatR;
using Microsoft.AspNetCore.Mvc;
using VaccinationCard.Application.UseCases.Persons.Commands.CreatePerson;
using VaccinationCard.Application.UseCases.Persons.Queries.GetPersonCard;
using VaccinationCard.Application.UseCases.Persons.Commands.DeletePerson;
using VaccinationCard.Application.UseCases.Persons.Commands.UpdatePerson;
using VaccinationCard.Application.DTOs;

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

    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)] // Mudou de 204 para 200
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(int id)
    {
        var command = new DeletePersonCommand(id);
        var deletedPerson = await _mediator.Send(command);

        if (deletedPerson == null)
        {
            return NotFound(new { message = "Person not found" });
        }

        // Retorna o objeto apagado
        return Ok(deletedPerson);
    }

    [HttpPut("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Update(int id, [FromBody] UpdatePersonRequest request)
    {
        var command = new UpdatePersonCommand(
            id, 
            request.Name, 
            request.Age, 
            request.Gender
        );

        var updatedPerson = await _mediator.Send(command);

        if (updatedPerson == null)
        {
            return NotFound(new { message = "Person not found" });
        }

        return Ok(updatedPerson);
    }
}