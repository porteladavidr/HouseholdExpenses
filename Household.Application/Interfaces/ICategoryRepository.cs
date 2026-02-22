using System;
using System.Collections.Generic;
using System.Text;
using Household.Domain.Entities;

namespace Household.Application.Interfaces;

public interface ICategoryRepository
{
    Task<Category?> GetByIdAsync(int id, CancellationToken ct);
    Task<List<Category>> ListAsync(CancellationToken ct);
    Task<Category> AddAsync(Category category, CancellationToken ct);
}