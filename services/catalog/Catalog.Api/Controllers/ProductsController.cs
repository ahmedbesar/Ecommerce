using Catalog.Api.Extensions;
using Catalog.Application.Commands;
using Catalog.Application.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Catalog.Api.Controllers;

[Route("api/products")]
[ApiController]
public class ProductsController : ControllerBase
{
    private readonly IMediator _mediator;

    public ProductsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<IResult> GetAll()
    {
        var result = await _mediator.Send(new GetAllProductsQuery());
        return result.ToHttpResponse();
    }

    [HttpGet("{id}")]
    public async Task<IResult> GetById(string id)
    {
        var result = await _mediator.Send(new GetProductByIdQuery { Id = id });
        return result.ToHttpResponse();
    }

    [HttpGet("name/{name}")]
    public async Task<IResult> GetByName(string name)
    {
        var result = await _mediator.Send(new GetProductByNameQuery { Name = name });
        return result.ToHttpResponse();
    }

    [HttpGet("brand/{brandName}")]
    public async Task<IResult> GetByBrand(string brandName)
    {
        var result = await _mediator.Send(new GetProductsByBrandQuery { BrandName = brandName });
        return result.ToHttpResponse();
    }

    [HttpPost]
    public async Task<IResult> Create([FromBody] CreateProductCommand command)
    {
        var result = await _mediator.Send(command);
        return result.ToHttpResponse();
    }

    [HttpPut]
    public async Task<IResult> Update([FromBody] UpdateProductCommand command)
    {
        var result = await _mediator.Send(command);
        return result.ToHttpResponse();
    }

    [HttpDelete("{id}")]
    public async Task<IResult> Delete(string id)
    {
        var result = await _mediator.Send(new DeleteProductCommand { Id = id });
        return result.ToHttpResponse();
    }

    [HttpDelete]
    [Route("delete-all")]
    public async Task<IResult> DeleteAll()
    {
        var result = await _mediator.Send(new DeleteAllProductsCommand());
        return result.ToHttpResponse();
    }
}
