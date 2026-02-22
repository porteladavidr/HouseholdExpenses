using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using Household.Domain.Enums;

namespace Household.Application.DTOs.Transactions;

public record TransactionCreateDto(
    [Required, MaxLength(400)] string Description,
    [Range(0.01, double.MaxValue)] decimal Value,
    TransactionType Type,
    int CategoryId,
    int PersonId
);