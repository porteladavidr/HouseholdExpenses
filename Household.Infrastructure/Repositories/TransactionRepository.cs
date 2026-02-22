using System;
using System.Collections.Generic;
using System.Text;
using Household.Application.Interfaces;
using Household.Domain.Entities;
using Household.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Household.Infrastructure.Repositories;

public class TransactionRepository : ITransactionRepository
{
    private readonly AppDbContext _db;
    public TransactionRepository(AppDbContext db) => _db = db;

    public Task<List<Transaction>> ListAsync(CancellationToken ct)
        => _db.Transactions
            .AsNoTracking()
            .Include(t => t.Category)
            .Include(t => t.Person)
            .OrderByDescending(t => t.Id)
            .ToListAsync(ct);

    public async Task<Transaction> AddAsync(Transaction transaction, CancellationToken ct)
    {
        _db.Transactions.Add(transaction);
        await _db.SaveChangesAsync(ct);
        return transaction;
    }

    public async Task DeleteByPersonIdAsync(int personId, CancellationToken ct)
    {
        var txs = await _db.Transactions.Where(t => t.PersonId == personId).ToListAsync(ct);
        _db.Transactions.RemoveRange(txs);
        await _db.SaveChangesAsync(ct);
    }
}