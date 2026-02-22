using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using Household.Domain.Enums;

namespace Household.Domain.Entities;

public class Transaction
{
    public int Id { get; set; }

    [Required, MaxLength(400)]
    public string Description { get; set; } = string.Empty;

    [Range(0.01, double.MaxValue)]
    public decimal Value { get; set; }

    public TransactionType Type { get; set; }

    // FKs
    public int CategoryId { get; set; }
    public Category? Category { get; set; }

    public int PersonId { get; set; }
    public Person? Person { get; set; }
}
