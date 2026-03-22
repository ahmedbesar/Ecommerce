using FluentResults;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;

namespace Ordering.Api.Extensions
{
    public static class ResultExtensions
    {
        public static ActionResult ToHttpResponse<T>(this Result<T> result)
        {
            if (result.IsSuccess)
                return new OkObjectResult(result.Value);

            return CreateBadRequest(result.Errors);
        }

        public static ActionResult ToHttpResponse(this Result result)
        {
            if (result.IsSuccess)
                return new OkResult();

            return CreateBadRequest(result.Errors);
        }

        private static BadRequestObjectResult CreateBadRequest(IEnumerable<IError> errors)
        {
            return new BadRequestObjectResult(new
            {
                Errors = errors.Select(e => e.Message)
            });
        }
    }
}
