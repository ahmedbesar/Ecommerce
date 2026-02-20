using Catalog.Api.Extensions;
using Catalog.Application.Commands;
using Catalog.Application.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Catalog.Api.Controllers;
public class CatalogController : BaseApiController
{
    private readonly IMediator _mediator;

    public CatalogController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("{id}")]
    public async Task<IResult> GetProductById(string id)
    {
        var result = await _mediator.Send(new GetProductByIdQuery { Id = id });
        return result.ToHttpResponse();
    }

    [HttpGet]
    public async Task<IResult> GetAllProducts()
    {
        var result = await _mediator.Send(new GetAllProductsQuery());
        return result.ToHttpResponse();
    }

    [HttpGet("brands")]
    public async Task<IResult> GetAllBrands()
    {
        var result = await _mediator.Send(new GetAllBrandsQuery());
        return result.ToHttpResponse();
    }

    [HttpGet("types")]
    public async Task<IResult> GetAllTypes()
    {
        var result = await _mediator.Send(new GetAllTypesQuery());
        return result.ToHttpResponse();
    }

    [HttpGet("name/{name}")]
    public async Task<IResult> GetProductsByName(string name)
    {
        var result = await _mediator.Send(new GetProductByNameQuery { Name = name });
        return result.ToHttpResponse();
    }

    [HttpGet("brand/{brandName}")]
    public async Task<IResult> GetProductsByBrand(string brandName)
    {
        var result = await _mediator.Send(new GetProductsByBrandQuery { BrandName = brandName });
        return result.ToHttpResponse();
    }

    [HttpPost]
    public async Task<IResult> CreateProduct([FromBody] CreateProductCommand command)
    {
        var result = await _mediator.Send(command);
        return result.ToHttpResponse();
    }

    [HttpPut]
    public async Task<IResult> UpdateProduct([FromBody] UpdateProductCommand command)
    {
        var result = await _mediator.Send(command);
        return result.ToHttpResponse();
    }

    [HttpDelete("{id}")]
    public async Task<IResult> DeleteProduct(string id)
    {
        var result = await _mediator.Send(new DeleteProductCommand { Id = id });
        return result.ToHttpResponse();
    }
}
