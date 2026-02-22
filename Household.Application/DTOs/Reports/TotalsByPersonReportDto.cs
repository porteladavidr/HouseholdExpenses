using System;
using System.Collections.Generic;
using System.Text;

namespace Household.Application.DTOs.Reports;

public record TotalsByPersonReportDto(
    List<TotalsByPersonItemDto> Itens,
    TotalsSummaryDto TotalGeral
);
