using System;
using System.Collections.Generic;
using System.Text;
using Household.Application.Interfaces;
using Household.Domain.Entities;
using Household.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Household.Infrastructure.Repositories;

public class CategoryRepository : ICategoryRepository
{
    private readonly AppDbContext _db;
    public CategoryRepository(AppDbContext db) => _db = db;

    public Task<Category?> GetByIdAsync(int id, CancellationToken ct)
        => _db.Categories.FirstOrDefaultAsync(c => c.Id == id, ct);

    public Task<List<Category>> ListAsync(CancellationToken ct)
        => _db.Categories.AsNoTracking().OrderBy(c => c.Description).ToListAsync(ct);

    public async Task<Category> AddAsync(Category category, CancellationToken ct)
    {
        _db.Categories.Add(category);
        await _db.SaveChangesAsync(ct);
        return category;
    }
}
