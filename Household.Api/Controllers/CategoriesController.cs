using Household.Application.DTOs.Categories;
using Household.Application.Interfaces;
using Household.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace Household.Api.Controllers;

[ApiController]
[Route("api/categorias")]
public class CategoriesController : ControllerBase
{
    private readonly ICategoryRepository _repo;

    public CategoriesController(ICategoryRepository repo) => _repo = repo;

    [HttpGet]
    public async Task<ActionResult<List<CategoryListDto>>> List(CancellationToken ct)
    {
        List<Category> categories = await _repo.ListAsync(ct);
        return categories.Select(c => new CategoryListDto(c.Id, c.Description, c.Purpose)).ToList();
    }

    [HttpPost]
    public async Task<ActionResult<CategoryListDto>> Create(CategoryCreateDto dto, CancellationToken ct)
    {
        Category category = new Category
        {
            Description = dto.Description.Trim(),
            Purpose = dto.Purpose
        };

        Category created = await _repo.AddAsync(category, ct);

        return CreatedAtAction(nameof(List), new { id = created.Id },
            new CategoryListDto(created.Id, created.Description, created.Purpose));
    }
}