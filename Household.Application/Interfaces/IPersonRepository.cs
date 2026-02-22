using System;
using System.Collections.Generic;
using System.Text;
using Household.Domain.Entities;

namespace Household.Application.Interfaces;

public interface IPersonRepository
{
    Task<Person?> GetByIdAsync(int id, CancellationToken ct);
    Task<List<Person>> ListAsync(CancellationToken ct);
    Task<Person> AddAsync(Person person, CancellationToken ct);
    Task UpdateAsync(Person person, CancellationToken ct);
    Task DeleteAsync(Person person, CancellationToken ct);
}
