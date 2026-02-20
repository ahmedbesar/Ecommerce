using Catalog.Application.Queries;
using Catalog.Application.Responses;
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

        public async Task<ProductResponseDto> GetProductById([FromBody] GetProductByIdQuery query)
        {
            var result = await _mediator.Send(query);
            return result;
        }
    }
}
