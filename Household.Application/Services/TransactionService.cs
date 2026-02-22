using System;
using System.Collections.Generic;
using System.Text;
using Household.Application.DTOs.Transactions;
using Household.Application.Exceptions;
using Household.Application.Interfaces;
using Household.Domain.Entities;
using Household.Domain.Enums;

namespace Household.Application.Services;

public class TransactionService
{
    private readonly IPersonRepository _people;
    private readonly ICategoryRepository _categories;
    private readonly ITransactionRepository _transactions;

    public TransactionService(
        IPersonRepository people,
        ICategoryRepository categories,
        ITransactionRepository transactions)
    {
        _people = people;
        _categories = categories;
        _transactions = transactions;
    }

    public async Task<Transaction> CreateAsync(TransactionCreateDto dto, CancellationToken ct)
    {
        var person = await _people.GetByIdAsync(dto.PersonId, ct)
            ?? throw new KeyNotFoundException("Pessoa não encontrada.");

        if (person.Age < 18 && dto.Type != TransactionType.Despesa)
            throw new BusinessRuleException("Menor de idade só pode lançar despesas.");

        var category = await _categories.GetByIdAsync(dto.CategoryId, ct)
            ?? throw new KeyNotFoundException("Categoria não encontrada.");

        var ok =
            dto.Type == TransactionType.Despesa
                ? category.Purpose is CategoryPurpose.Despesa or CategoryPurpose.Ambas
                : category.Purpose is CategoryPurpose.Receita or CategoryPurpose.Ambas;

        if (!ok)
            throw new BusinessRuleException("Categoria incompatível com o tipo da transação.");

        var tx = new Transaction
        {
            Description = dto.Description.Trim(),
            Value = dto.Value,
            Type = dto.Type,
            CategoryId = dto.CategoryId,
            PersonId = dto.PersonId
        };

        return await _transactions.AddAsync(tx, ct);
    }
}
