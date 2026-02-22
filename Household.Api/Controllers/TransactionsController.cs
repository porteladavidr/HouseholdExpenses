using Household.Application.DTOs.Transactions;
using Household.Application.Interfaces;
using Household.Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace Household.Api.Controllers;

[ApiController]
[Route("api/transacoes")]
public class TransactionsController : ControllerBase
{
    private readonly TransactionService _service;
    private readonly ITransactionRepository _repo;

    public TransactionsController(TransactionService service, ITransactionRepository repo)
    {
        _service = service;
        _repo = repo;
    }

    [HttpGet]
    public async Task<ActionResult<List<TransactionListDto>>> List(CancellationToken ct)
    {
        var list = await _repo.ListAsync(ct);

        return list.Select(t => new TransactionListDto(
            t.Id,
            t.Description,
            t.Value,
            t.Type,
            t.CategoryId,
            t.Category?.Description ?? "",
            t.PersonId,
            t.Person?.Name ?? ""
        )).ToList();
    }

    [HttpPost]
    public async Task<IActionResult> Create(TransactionCreateDto dto, CancellationToken ct)
    {
        var created = await _service.CreateAsync(dto, ct);
        return CreatedAtAction(nameof(List), new { id = created.Id }, new { created.Id });
    }
}