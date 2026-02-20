using Catalog.Api.Extensions;
using Catalog.Application.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Catalog.Api.Controllers;

[Route("api/types")]
[ApiController]
public class TypesController : ControllerBase
{
    private readonly IMediator _mediator;

    public TypesController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<IResult> GetAll()
    {
        var result = await _mediator.Send(new GetAllTypesQuery());
        return result.ToHttpResponse();
    }
}
