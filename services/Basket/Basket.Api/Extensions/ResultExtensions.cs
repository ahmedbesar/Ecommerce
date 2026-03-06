using FluentResults;
using Microsoft.AspNetCore.Mvc;

namespace Basket.Api.Extensions;

public static class ResultExtensions
{
    public static IResult ToHttpResponse<T>(this Result<T> result)
    {
        if (result.IsSuccess)
            return Results.Ok(result.Value);

        return Results.BadRequest(new { Errors = result.Errors.Select(e => e.Message) });
    }

    public static IResult ToHttpResponse(this Result result)
    {
        if (result.IsSuccess)
            return Results.Ok();

        return Results.BadRequest(new { Errors = result.Errors.Select(e => e.Message) });
    }
}
