using MediatR;
using Microsoft.AspNetCore.Mvc;
using VaccinationCard.Application.UseCases.Persons.Commands.CreatePerson;
using VaccinationCard.Application.UseCases.Persons.Queries.GetPersonCard;
using VaccinationCard.Application.UseCases.Persons.Queries.GetAllPersons;
using VaccinationCard.Application.UseCases.Persons.Commands.DeletePerson;
using VaccinationCard.Application.UseCases.Persons.Commands.UpdatePerson;
using VaccinationCard.Application.DTOs;
using Microsoft.AspNetCore.Authorization;

namespace VaccinationCard.Api.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class PersonsController : ControllerBase
{
    private readonly IMediator _mediator;

    public PersonsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    // POST
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create([FromBody] CreatePersonCommand command)
    {
        var response = await _mediator.Send(command);

        // Retorna 201 Created com o corpo da resposta
        return CreatedAtAction(nameof(Create), new { id = response.Id }, response);
    }

    // GET By Id
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

    // GET All
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll()
    {
        var query = new GetAllPersonsQuery();
        var result = await _mediator.Send(query);
        
        return Ok(result);
    }

    // DELETE
    [HttpDelete("{id}")]
    [Authorize(Roles = "ADMIN")]
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

    // PUT (Update)
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