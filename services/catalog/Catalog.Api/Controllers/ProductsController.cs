using Catalog.Api.Extensions;
using Catalog.Application.Commands;
using Catalog.Application.Queries;
using Catalog.Core.Specifications;
using Catalog.Core.Specifications.Products;
using Common.Authentication;
using Common.Authentication.Consts;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Catalog.Api.Controllers;

public class ProductsController : BaseApiController
{
    private readonly IMediator _mediator;

    public ProductsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<ActionResult> GetAll([FromQuery] ProductSpecificationParams specParams)
    {
        var result = await _mediator.Send(new GetAllProductsQuery(specParams));
        return result.ToHttpResponse();
    }

    [HttpGet("{id}")]
    public async Task<ActionResult> GetById(string id)
    {
        var result = await _mediator.Send(new GetProductByIdQuery { Id = id });
        return result.ToHttpResponse();
    }

   
    [HttpGet("name/{name}")]
    public async Task<ActionResult> GetByName(string name)
    {
        var result = await _mediator.Send(new GetProductByNameQuery { Name = name });
        return result.ToHttpResponse();
    }

    [HttpGet("brand/{brandName}")]
    public async Task<ActionResult> GetByBrand(string brandName)
    {
        var result = await _mediator.Send(new GetProductsByBrandQuery { BrandName = brandName });
        return result.ToHttpResponse();
    }

    [HttpPost]
    public async Task<ActionResult> Create([FromBody] CreateProductCommand command)
    {
        var result = await _mediator.Send(command);
        return result.ToHttpResponse();
    }

    [HttpPut]
    public async Task<ActionResult> Update([FromBody] UpdateProductCommand command)
    {
        var result = await _mediator.Send(command);
        return result.ToHttpResponse();
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> Delete(string id)
    {
        var result = await _mediator.Send(new DeleteProductCommand { Id = id });
        return result.ToHttpResponse();
    }

    [HttpDelete]
    [Route("delete-all")]
    public async Task<ActionResult> DeleteAll()
    {
        var result = await _mediator.Send(new DeleteAllProductsCommand());
        return result.ToHttpResponse();
    }
}
