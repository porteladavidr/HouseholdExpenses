using System;
using System.Collections.Generic;
using System.Text;
using Household.Application.DTOs.Reports;

namespace Household.Application.Interfaces;

public interface IReportRepository
{
    Task<TotalsByPersonReportDto> GetTotalsByPersonAsync(CancellationToken ct);
}
