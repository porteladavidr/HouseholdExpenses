using System;
using System.Collections.Generic;
using System.Text;
using Household.Application.Interfaces;
using Household.Domain.Entities;
using Household.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Household.Infrastructure.Repositories;

public class PersonRepository : IPersonRepository
{
    private readonly AppDbContext _db;
    public PersonRepository(AppDbContext db) => _db = db;

    public Task<Person?> GetByIdAsync(int id, CancellationToken ct)
        => _db.People.FirstOrDefaultAsync(p => p.Id == id, ct);

    public Task<List<Person>> ListAsync(CancellationToken ct)
        => _db.People.AsNoTracking().OrderBy(p => p.Name).ToListAsync(ct);

    public async Task<Person> AddAsync(Person person, CancellationToken ct)
    {
        _db.People.Add(person);
        await _db.SaveChangesAsync(ct);
        return person;
    }

    public async Task UpdateAsync(Person person, CancellationToken ct)
    {
        _db.People.Update(person);
        await _db.SaveChangesAsync(ct);
    }

    public async Task DeleteAsync(Person person, CancellationToken ct)
    {
        _db.People.Remove(person);
        await _db.SaveChangesAsync(ct);
    }
}