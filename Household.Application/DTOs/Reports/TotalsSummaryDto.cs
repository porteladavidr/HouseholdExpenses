using System;
using System.Collections.Generic;
using System.Text;

namespace Household.Application.DTOs.Reports;

public record TotalsSummaryDto(decimal TotalReceitas, decimal TotalDespesas, decimal Saldo);
