using System;
using System.Collections.Generic;
using System.Text;
using Household.Application.DTOs.Reports;
using Household.Application.Interfaces;
using Household.Domain.Enums;
using Household.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Household.Infrastructure.Repositories;

public class ReportRepository : IReportRepository
{
    private readonly AppDbContext _db;
    public ReportRepository(AppDbContext db) => _db = db;

    public async Task<TotalsByPersonReportDto> GetTotalsByPersonAsync(CancellationToken ct)
    {
        var people = await _db.People
            .AsNoTracking()
            .Select(p => new
            {
                p.Id,
                p.Name,
                p.Age
            })
            .ToListAsync(ct);

        var totals = await _db.Transactions
            .AsNoTracking()
            .GroupBy(t => t.PersonId)
            .Select(g => new
            {
                PersonId = g.Key,
                TotalReceitas = g.Where(x => x.Type == TransactionType.Receita).Sum(x => x.Value),
                TotalDespesas = g.Where(x => x.Type == TransactionType.Despesa).Sum(x => x.Value)
            })
            .ToListAsync(ct);

        var map = totals.ToDictionary(x => x.PersonId);

        var itens = people.Select(p =>
        {
            map.TryGetValue(p.Id, out var t);

            var receitas = t?.TotalReceitas ?? 0m;
            var despesas = t?.TotalDespesas ?? 0m;

            return new TotalsByPersonItemDto(
                p.Id,
                p.Name,
                p.Age,
                receitas,
                despesas,
                receitas - despesas
            );
        })
        .OrderBy(x => x.Name)
        .ToList();

        var totalReceitasGeral = itens.Sum(x => x.TotalReceitas);
        var totalDespesasGeral = itens.Sum(x => x.TotalDespesas);

        return new TotalsByPersonReportDto(
            itens,
            new TotalsSummaryDto(totalReceitasGeral, totalDespesasGeral, totalReceitasGeral - totalDespesasGeral)
        );
    }
}