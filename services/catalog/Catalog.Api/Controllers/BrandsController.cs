using Catalog.Api.Extensions;
using Catalog.Application.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Catalog.Api.Controllers;

[AllowAnonymous]
public class BrandsController : BaseApiController
{
    private readonly IMediator _mediator;

    public BrandsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<ActionResult> GetAll()
    {
        var result = await _mediator.Send(new GetAllBrandsQuery());
        return result.ToHttpResponse();
    }
}
