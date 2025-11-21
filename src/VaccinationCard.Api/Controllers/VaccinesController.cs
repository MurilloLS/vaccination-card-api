using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VaccinationCard.Infrastructure.Persistence;

namespace VaccinationCard.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class VaccinesController : ControllerBase
{
    private readonly VaccinationDbContext _context;

    public VaccinesController(VaccinationDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var vaccines = await _context.Vaccines
            .Include(v => v.Category)
            .Select(v => new 
            { 
                v.Id, 
                Vaccine = v.Name, 
                Category = v.Category.Name 
            })
            .ToListAsync();

        return Ok(vaccines);
    }
}