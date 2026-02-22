using Microsoft.AspNetCore.Mvc;
using Household.Application.DTOs.People;
using Household.Application.Interfaces;
using Household.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace Household.Api.Controllers;

[ApiController]
[Route("api/pessoas")]
public class PeopleController : ControllerBase
{
    private readonly IPersonRepository _repo;

    public PeopleController(IPersonRepository repo) => _repo = repo;

    [HttpGet]
    public async Task<ActionResult<List<PersonListDto>>> List(CancellationToken ct)
    {
        List<Person> people = await _repo.ListAsync(ct);
        return people.Select(p => new PersonListDto(p.Id, p.Name, p.Age)).ToList();
    }

    [HttpPost]
    public async Task<ActionResult<PersonListDto>> Create(PersonCreateDto dto, CancellationToken ct)
    {
        Person person = new Person { Name = dto.Name.Trim(), Age = dto.Age };
        Person created = await _repo.AddAsync(person, ct);

        return CreatedAtAction(nameof(GetById),
            new { id = created.Id },
            new PersonListDto(created.Id, created.Name, created.Age));
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<PersonListDto>> GetById(int id, CancellationToken ct)
    {
        Person? Person = await _repo.GetByIdAsync(id, ct);
        if (Person is null) return NotFound();

        return new PersonListDto(Person.Id, Person.Name, Person.Age);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, PersonUpdateDto dto, CancellationToken ct)
    {
        Person? Person = await _repo.GetByIdAsync(id, ct);
        if (Person is null) return NotFound();

        Person.Name = dto.Name.Trim();
        Person.Age = dto.Age;

        await _repo.UpdateAsync(Person, ct);
        return NoContent();
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id, CancellationToken ct)
    {
        Person? Person = await _repo.GetByIdAsync(id, ct);
        if (Person is null) return NotFound();

        await _repo.DeleteAsync(Person, ct);
        return NoContent();
    }
}