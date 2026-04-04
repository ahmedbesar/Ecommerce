using Catalog.Api.Extensions;
using Catalog.Application.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Catalog.Api.Controllers;

public class TypesController : BaseApiController
{
    private readonly IMediator _mediator;

    public TypesController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<ActionResult> GetAll()
    {
        var result = await _mediator.Send(new GetAllTypesQuery());
        return result.ToHttpResponse();
    }
}
