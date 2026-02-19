using Catalog.Application.Responses;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Catalog.Application.Queries
{
    public sealed record GetProductByIdQuery : IRequest<ProductResponseDto>
    {
        public GetProductByIdQuery(string id)
        {
            Id = id;
        }

        public string Id { get; set; }
    }
}
