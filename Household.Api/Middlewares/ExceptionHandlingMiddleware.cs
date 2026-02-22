using Household.Application.Exceptions;
using Microsoft.AspNetCore.Mvc;

namespace Household.Api.Middlewares;

public class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;

    public ExceptionHandlingMiddleware(RequestDelegate next) => _next = next;

    public async Task Invoke(HttpContext ctx)
    {
        try
        {
            await _next(ctx);
        }
        catch (BusinessRuleException ex)
        {
            ctx.Response.StatusCode = StatusCodes.Status400BadRequest;
            await ctx.Response.WriteAsJsonAsync(new ProblemDetails
            {
                Title = "Regra de negócio violada",
                Detail = ex.Message,
                Status = 400
            });
        }
        catch (KeyNotFoundException ex)
        {
            ctx.Response.StatusCode = StatusCodes.Status404NotFound;
            await ctx.Response.WriteAsJsonAsync(new ProblemDetails
            {
                Title = "Recurso não encontrado",
                Detail = ex.Message,
                Status = 404
            });
        }
    }
}