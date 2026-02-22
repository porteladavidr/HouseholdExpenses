using System;
using System.Collections.Generic;
using System.Text;
namespace Household.Application.DTOs.Reports;

public record TotalsByPersonItemDto(
    int PersonId,
    string Name,
    int Age,
    decimal TotalReceitas,
    decimal TotalDespesas,
    decimal Saldo
);
