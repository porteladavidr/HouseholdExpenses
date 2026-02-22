using System;
using System.Collections.Generic;
using System.Text;
using Household.Domain.Entities;

namespace Household.Application.Interfaces;

public interface ITransactionRepository
{
    Task<List<Transaction>> ListAsync(CancellationToken ct);
    Task<Transaction> AddAsync(Transaction transaction, CancellationToken ct);
    Task DeleteByPersonIdAsync(int personId, CancellationToken ct);
}
