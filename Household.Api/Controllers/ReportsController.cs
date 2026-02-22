using Household.Application.DTOs.Reports;
using Household.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Household.Api.Controllers;

[ApiController]
[Route("api/relatorios")]
public class ReportsController : ControllerBase
{
    private readonly IReportRepository _reports;

    public ReportsController(IReportRepository reports) => _reports = reports;

    [HttpGet("totais-por-pessoa")]
    public async Task<ActionResult<TotalsByPersonReportDto>> TotaisPorPessoa(CancellationToken ct)
        => await _reports.GetTotalsByPersonAsync(ct);
}