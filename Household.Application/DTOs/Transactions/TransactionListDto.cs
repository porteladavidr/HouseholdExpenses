using System;
using System.Collections.Generic;
using System.Text;
using Household.Domain.Enums;

namespace Household.Application.DTOs.Transactions;

public record TransactionListDto(
    int Id,
    string Description,
    decimal Value,
    TransactionType Type,
    int CategoryId,
    string CategoryDescription,
    int PersonId,
    string PersonName
);