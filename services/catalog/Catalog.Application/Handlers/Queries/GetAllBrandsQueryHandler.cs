using Catalog.Application.Mappers;
using Catalog.Application.Queries;
using Catalog.Application.Responses;
using Catalog.Core.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Catalog.Application.Handlers.Queries
{
    public class GetAllBrandsQueryHandler : IRequestHandler<GetAllBrandsQuery, IEnumerable<ProductBrandResponse>>
    {
        private readonly IBrandRepository _repository;

        public GetAllBrandsQueryHandler(IBrandRepository repository)
        {
            _repository = repository;
        }
        public async Task<IEnumerable<ProductBrandResponse>> Handle(GetAllBrandsQuery request, CancellationToken cancellationToken)
        {
            var brands = await _repository.GetAllBrands();
            var mapper = new BrandMapper();
            var response = mapper.ToResponseList(brands);
            return response;
        }
    }
}
