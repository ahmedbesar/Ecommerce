using FluentResults;
using Microsoft.AspNetCore.Mvc;

namespace Catalog.Api.Extensions;

public static class ResultExtensions
{
    public static IResult ToHttpResponse<T>(this Result<T> result)
    {
        if (result.IsSuccess)
            return Results.Ok(result.Value);

        return result.Errors.FirstOrDefault()?.Message switch
        {
            var msg when msg?.Contains("not found", StringComparison.OrdinalIgnoreCase) == true 
                => Results.NotFound(new { Errors = result.Errors.Select(e => e.Message) }),
            _ => Results.BadRequest(new { Errors = result.Errors.Select(e => e.Message) })
        };
    }

    public static IResult ToHttpResponse(this Result result)
    {
        if (result.IsSuccess)
            return Results.Ok();

        return result.Errors.FirstOrDefault()?.Message switch
        {
            var msg when msg?.Contains("not found", StringComparison.OrdinalIgnoreCase) == true 
                => Results.NotFound(new { Errors = result.Errors.Select(e => e.Message) }),
            _ => Results.BadRequest(new { Errors = result.Errors.Select(e => e.Message) })
        };
    }
}
