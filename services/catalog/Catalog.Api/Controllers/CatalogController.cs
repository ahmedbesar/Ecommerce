using Catalog.Application.Queries;
using Catalog.Application.Responses;
using FluentResults;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Catalog.Api.Controllers
{
    public class CatalogController : BaseApiController
    {
        private readonly IMediator _mediator;

        public CatalogController(IMediator mediator)
        {
            _mediator = mediator;
        }
        [HttpGet]
        public async Task<Result<ProductResponseDto> > GetProductById(GetProductByIdQuery query)
        {
            var result = await _mediator.Send(query);
            return result;
        }
    }
}
